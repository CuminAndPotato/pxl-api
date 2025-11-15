[<AutoOpen>]
module Pxl.PxlLocalDev

open System
open System.IO
open System.Reflection

[<RequireQualifiedAccess>]
module Asset =
    let loadFromAssembly (assetName: string) (assembly: Assembly) =
        let assetName = assetName.Replace(" ", "_")
        // "assets" is omitted somwhow - but only in pre-compiled apps. Why? Who knows :(
        let resourceNames =
            [
                $"{assembly.GetName().Name}.assets.{assetName}"
                $"{assembly.GetName().Name}.{assetName}"
            ]
        let res =
            resourceNames
            |> List.map (fun resourceName -> assembly.GetManifestResourceStream(resourceName))
            |> List.filter (fun stream -> stream <> null)
            |> List.tryHead
        match res with
        | None ->
            let existingNames = assembly.GetManifestResourceNames() |> String.concat ", "
            let resourceNamesString = resourceNames |> String.concat ", "
            failwith $"Asset not found: {assetName} -- Assembly name: {assembly.GetName().Name} -- resource names: {resourceNamesString}. Existing names: {existingNames}"
        | Some stream -> stream

    /// This function must only called from the assembly that contains the assets.
    let load (sourceDir: string, assetName: string) =
        if ApiEnv.isInInteractiveContext then
            let path = Path.Combine(sourceDir, "assets", assetName)
            let content = File.ReadAllBytes(path)
            new MemoryStream(content) :> Stream
        else
            let assembly = Assembly.GetCallingAssembly()
            loadFromAssembly assetName assembly


[<RequireQualifiedAccess>]
module Image =

    let internal load (sourceDir: string) (assetName: string) f =
        if ApiEnv.isInInteractiveContext then
            Asset.load(sourceDir, assetName)
            |> f
        else
            Asset.loadFromAssembly assetName (Assembly.GetCallingAssembly())
            |> f

    /// This function must only called from the assembly that contains the assets.
    let loadFromAsset (sourceDir: string, assetName: string) =
        load sourceDir assetName Pxl.Ui.Image.load

    let loadFramesFromAsset (sourceDir: string, assetName: string) =
        load sourceDir assetName Pxl.Ui.Image.loadFrames


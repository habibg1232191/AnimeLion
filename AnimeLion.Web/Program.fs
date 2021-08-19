module AnimeLion.Web.App

open System
open System.IO
open System.Data.SQLite
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

// ---------------------------------
// Models
// ---------------------------------

[<CLIMutable>]
type TopAnimeCurrentYear =
    {
        Id : int
        ShikimoriId : int
        Name : string
        ImageUrl : string
        Description : string
    }

// ---------------------------------
// Views
// ---------------------------------

//module Views =
//    open Giraffe.ViewEngine
//
//    let layout (content: XmlNode list) =
//        html [] [
//            head [] [
//                title []  [ encodedText "AnimeLion.Web" ]
//                link [ _rel  "stylesheet"
//                       _type "text/css"
//                       _href "/main.css" ]
//            ]
//            body [] content
//        ]
//
//    let partial () =
//        h1 [] [ encodedText "AnimeLion.Web" ]
//
//    let index (model : Message) =
//        [
//            partial()
//            p [] [ encodedText model.Text ]
//        ] |> layout

// ---------------------------------
// Web app
// ---------------------------------

//let indexHandler (name : string) =
//    let greetings = sprintf "Hello %s, from Giraffe!" name
//    let model     = { Text = greetings }
//    let view      = Views.index model
//    htmlView view

let topAnimeCurrentYearHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        let mutable response:list<TopAnimeCurrentYear> = []
        
        let databaseFilename = Path.Combine(Directory.GetCurrentDirectory(), "animelion.db")
        let connectionString = $"Data Source=%s{databaseFilename};Version=3;"
        let existFile = File.Exists(databaseFilename)
        if not existFile then SQLiteConnection.CreateFile(databaseFilename)

        let connection = new SQLiteConnection(connectionString)
        connection.Open()

        let selectSql = "select * from top_anime_current_year"
        let selectCommand = new SQLiteCommand(selectSql, connection)
        let reader = selectCommand.ExecuteReader()

        while reader.Read() do
            response <- response|>List.append([{
                Id = reader.["id"].ToString()|>int
                ShikimoriId = reader.["shikimori_id"].ToString()|>int
                Name = reader.["name"].ToString()
                ImageUrl = reader.["image_url"].ToString()
                Description = reader.["description"].ToString()
            }])
        
        connection.Close()
        json response next ctx

let webApp =
    choose [
        GET >=>
            choose [
                subRoute "/api" (choose[
                    GET >=> choose [
                        route "/top_anime_current_year" >=> topAnimeCurrentYearHandler
                    ]
                ])
            ]
        setStatusCode 404 >=> text "Not Found"
    ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins(
            "http://localhost:5000",
            "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection())
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(webRoot)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    0
using Serilog;


var builder = WebApplication.CreateBuilder(args);


// ******************** SETUP SERILOG 1/2 ********************
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
// ***********************************************************


var app = builder.Build();


// ************** DEMO UTILISATION SERILOG DANS DEUX ENDPOINT FORMAT MINIMAL API ***************

app.MapGet("/", () =>
{
    Log.Information("Je log l'info que le endpoint à été call");
    return TypedResults.Ok(new { Message = "Hello, World!"} );
});


app.MapGet("/error", () =>
{
    Log.Error("Je log une erreur car l'utilisateur à tenter un truc interdit :O");
    return TypedResults.BadRequest(new { Message = "Pas d'inspiration .." });
});
// **********************************************************************************************


app.UseSerilogRequestLogging(); // <-----



// ******************** SETUP SERILOG 2/2 ********************
try
{
    // Journalisez un message d'info
    Log.Information("Starting web application"); // 

    // Exécutez l'application
    app.Run();
}
catch (Exception ex)
{
    // Journalisez un message "Fatal" avec l'exception
    Log.Fatal(ex, "Application à planter");
}
finally
{
    // Fermez et videz le logger SERILOG pour s'assurer que tous les logs sont correctement enregistrés
    Log.CloseAndFlush();
}
using Clamify.Web;

var app = ClamifyWebApplicationBuilderProvider
    .Get(args)
    .Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthChecks("/health/live");
app.UseCors("AllowAllPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();

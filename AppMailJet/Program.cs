using EmailService.Common.Email.Model;
using EmailService.HostedServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<EmailHostedService>();
builder.Services.AddHostedService(provider => provider.GetService<EmailHostedService>());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/envoi-mail", async (EmailHostedService hostedService) =>
{
     string[] contacts = { "barrybagata96@gmail.com", "barryfreres224@gmail.com", "aguibou.barry@pointbase.fr"};

    for (int i = 0; i < contacts.Length; i++)
    {

        await hostedService.SendEmailAsync(new EmailModel
        {


            //EmailAdresse = "virginie.hugnet@pointbase.fr",

            EmailAdresse = contacts[i],
            Subject = "Mailing Test N°021 ",
            Body = "<strong> MAILJET SERVICE + DOTNET 6 </strong>" +
          " <p> Bonjour , ceci est un test d'envoi de mail" +
      " <br/> <b>Merci de ne pas repondre à ce message, il à été généré automatiquement !!!</b> </p>",
            Attachements = null

        });
    }

  
}).WithName("PointBase");




app.Run();


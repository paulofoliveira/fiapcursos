using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using Fiap.Cursos.Mobile.API.Models;
using Owin;
using Fiap.Cursos.Mobile.API.DataObjects;
using System.Data.Entity.Validation;

// FONTE: http://stackoverflow.com/questions/19664257/why-in-web-api-returning-an-entity-that-has-a-one-to-many-relationship-causes-an#comment65543534_22000303


namespace Fiap.Cursos.Mobile.API
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            //json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new MobileServiceInitializer());

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);
        }
    }

    public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            // Adicionando disciplinas:

            try
            {
                List<Disciplina> disciplinas = new List<Disciplina>
            {
               new Disciplina("Disciplina #A") { Id=Guid.NewGuid().ToString() },
               new Disciplina("Disciplina #B"){ Id=Guid.NewGuid().ToString() },
               new Disciplina("Disciplina #C"){ Id=Guid.NewGuid().ToString() },
               new Disciplina("Disciplina #D"){ Id=Guid.NewGuid().ToString() },
               new Disciplina("Disciplina #E"){ Id=Guid.NewGuid().ToString() }
            };

                foreach (Disciplina disciplina in disciplinas)
                {
                    context.Set<Disciplina>().Add(disciplina);
                }


                // Adicionando Cursos

                List<Curso> cursos = new List<Curso>
            {
               new Curso("MBA em .NET", "MBA em .NET"){ Id=Guid.NewGuid().ToString() },
               new Curso("MBA em BigData", "MBA em BigData"){ Id=Guid.NewGuid().ToString() },
               new Curso("Engenharia de Software", "Engenharia de Software"){ Id=Guid.NewGuid().ToString() }
            };

                foreach (Curso curso in cursos)
                {
                    curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[0], 1) { Id = Guid.NewGuid().ToString() });
                    curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[1], 1) { Id = Guid.NewGuid().ToString() });
                    curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[2], 2) { Id = Guid.NewGuid().ToString() });
                    curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[3], 2) { Id = Guid.NewGuid().ToString() });
                    curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[4], 3) { Id = Guid.NewGuid().ToString() });

                    context.Set<Curso>().Add(curso);
                }


                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var x = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    x += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:\n";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        x += $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n";
                    }
                }
                throw new Exception(x);
            }


            base.Seed(context);
        }
    }
}


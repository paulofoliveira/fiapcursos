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
using System.Linq;

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
                List<Disciplina> disciplinas = new List<Disciplina>();
                List<Curso> cursos = new List<Curso>();

                disciplinas.Add(new Disciplina("Disciplina #A") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #B") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #C") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #D") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #E") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #F") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #G") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #H") { Id = Guid.NewGuid().ToString() });
                disciplinas.Add(new Disciplina("Disciplina #I") { Id = Guid.NewGuid().ToString() });

                foreach (Disciplina disciplina in disciplinas)
                {
                    context.Set<Disciplina>().Add(disciplina);
                }

                // Adicionando Cursos:

                cursos.Add(new Curso("MBA em .NET", "Curso de .NET") { Id = Guid.NewGuid().ToString() });
                cursos.Add(new Curso("MBA em BigData", "Curso de BigData") { Id = Guid.NewGuid().ToString() });
                cursos.Add(new Curso("Engenharia de Software", "Curso de Engenharia de Software") { Id = Guid.NewGuid().ToString() });

                Random random;



                foreach (Curso curso in cursos)
                {
                    for (int i = 0; i < disciplinas.Count; i++)
                    {
                        random = new Random(DateTime.Now.Millisecond);
                        int proximoModulo = curso.Disciplinas.Max(p => p.Modulo) + 1;
                        byte modulo = i == 0 ? (byte)1 : (byte)random.Next(1, proximoModulo);
                        curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[i], modulo) { Id = Guid.NewGuid().ToString() });
                    }

                    context.Set<Curso>().Add(curso);
                }


                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var exceptionString = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    exceptionString += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:\n";

                    foreach (var ve in eve.ValidationErrors)
                        exceptionString += $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n";
                }

                throw new Exception(exceptionString);
            }

            base.Seed(context);
        }
    }
}


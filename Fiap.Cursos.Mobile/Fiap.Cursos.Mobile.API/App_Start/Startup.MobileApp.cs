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

                for (int i = 1; i <= 20; i++)
                    disciplinas.Add(new Disciplina($"Disciplina #{i}") { Id = Guid.NewGuid().ToString() });


                foreach (Disciplina disciplina in disciplinas)
                    context.Set<Disciplina>().Add(disciplina);

                // Adicionando Cursos:

                for (int i = 1; i <= 10; i++)
                    cursos.Add(new Curso($"Curso #{i}", $"Detalhes do curso #{i}.") { Id = Guid.NewGuid().ToString() });

                foreach (Curso curso in cursos)
                    context.Set<Curso>().Add(curso);

                foreach (Curso curso in cursos)
                {
                    int totalSorteado = new Random(DateTime.Now.Millisecond).Next(2, disciplinas.Count());
                    int qtdCursosPorModulo = new Random(DateTime.Now.Millisecond).Next(2, 5);
                    int adicionado = 0;
                    byte moduloCorrente = 0;

                    while (totalSorteado != adicionado)
                    {
                        moduloCorrente++;
                        List<int> disciplinasAdicionadas = new List<int>();
                        for (int i = 0; i < qtdCursosPorModulo; i++)
                        {
                            if (totalSorteado == adicionado) break;
                            int disciplina = 0;

                            while (disciplinasAdicionadas.Any() || disciplinasAdicionadas.Any(p => p == disciplina))
                                disciplina = new Random(DateTime.Now.Millisecond).Next(0, disciplinas.Count() - 1);

                            curso.Disciplinas.Add(new CursoDisciplina(curso, disciplinas[disciplina], moduloCorrente) { Id = Guid.NewGuid().ToString() });

                            adicionado++;
                        }
                    }
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
            catch (Exception ex)
            {
                throw ex;
            }

            base.Seed(context);
        }
    }
}


using System;
using System.Configuration;
using DreamSongs.MongoRepository;
using StructureMap;

namespace Blog.Web.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });

                            string mongoCon = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;
                            if (string.IsNullOrEmpty(mongoCon))
                                throw new ArgumentException("No MongoDb connection string specified");

                            x.For(typeof (IRepository<>)).Use(typeof (MongoRepository<>))
                                .CtorDependency<string>("connectionString")
                                .Is(mongoCon);

                        });

            return ObjectFactory.Container;
        }
    }
}
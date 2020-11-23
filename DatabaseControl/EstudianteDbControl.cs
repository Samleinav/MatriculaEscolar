using Couchbase.Lite;
using Couchbase.Lite.Query;
using Couchbase.Lite.Sync;
using System;
using System.Linq;
using System.Collections.Generic;
using ToolsBaseApp;
using ModelsBaseApp;

namespace DatabaseControl
{
    public class EstudianteDbControl
    {
        public EstudianteDbControl()
        {
            //var dic = AppDomain.CurrentDomain.BaseDirectory;
            //ConfigDB = new Database("config", new DatabaseConfiguration() { Directory = dic + "Config" });
            //database = new Database("estudiantes", new DatabaseConfiguration() { Directory = dic + "Estudiantes" });

        }

        protected Database databaseAll { get { return new Database("all_estudiantes", new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Estudiantes" }); } set { databaseAll = value; } }

        protected Database database { get { return new Database("estudiantes", new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Estudiantes" }); } set { database = value; } }
        private Database GenerateOrLoadDatabase(string name)
        {
            database = new Database(name, new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Estudiantes" });
            return database;
        }

        private MutableDocument CreateDocument(MutableDocument mutableDoc, string Event = "Create_Estudiante")
        {
            var Cedula = mutableDoc.GetString("Cedula");

            using (var docExist = database.GetDocument(Cedula))
            {
                if (docExist!= null)
                {
                    Event = "Get_Exists_Estudiante";
                    using (var bitacora = new BinnacleDBControl(Event))
                    {
                        bitacora.Add_Binnacle_Event(null, TypeBinnacle.Consult);
                        if (bitacora.IsBinnacleCreated)
                        {
                            return docExist.ToMutable();
                        }
                    }      
                }
            }

            if (globalEstudiantesDbControl.ExistEstudiante(Cedula))
            {
                var docExist = globalEstudiantesDbControl.GetEstudiante(Cedula);
                Event = "Create_Exists_Estudiante";
                mutableDoc = docExist.ToMutable();
            }

            using (var bitacora = new BinnacleDBControl(Event))
            {
                bitacora.Add_Binnacle_Event(mutableDoc, TypeBinnacle.Create);
                if (bitacora.IsBinnacleCreated)
                {
                    mutableDoc.SetString("IdlastBinnacle", bitacora.IdBinnacle);
                    database.Save(mutableDoc);
                    databaseAll.Save(mutableDoc);
                    return mutableDoc;
                }
                return null;
            }

        }
        private string UpdateDocument(MutableDocument mutableDocToSave, string Event = "Update_Estudiante")
        {
            using (var doc = database.GetDocument(mutableDocToSave.Id))
            {
                using (var bitacora = new BinnacleDBControl(Event))
                {
                    bitacora.Add_Binnacle_Event(mutableDocToSave, TypeBinnacle.Update);
                    if (bitacora.IsBinnacleCreated)
                    {
                        mutableDocToSave.SetString("IdlastBinnacle", bitacora.IdBinnacle);
                        database.Save(mutableDocToSave);
                        databaseAll.Save(mutableDocToSave);
                        return mutableDocToSave.Id;
                    }
                    return null;
                }

            }
        }
        private bool Deletedocument(string IdDoc, string Event = "Detele_Estudiante")
        {
            using(var docToDelete = database.GetDocument(IdDoc))
            {
                if(docToDelete != null)
                {
                    using (var bitacora = new BinnacleDBControl(Event))
                    {
                        bitacora.Add_Binnacle_Event(docToDelete.ToMutable(), TypeBinnacle.Detele);
                        if (bitacora.IsBinnacleCreated)
                        {
                            database.Delete(docToDelete);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private bool DocumentExist(string DocId)
        {
            return (database.GetDocument(DocId) != null);
        }
        public bool ExistEstudiante(string Cedula)
        {
           return DocumentExist(Cedula);
        }
       
        //public MethodResponse<EstudianteClass> CreateEstudiante(EstudianteClass datos)
        //{

        //    using (var estudiante = new MutableDocument(datos.Cedula))
        //    {
        //        estudiante.SetData(ConvertTo.ToDictionary(datos));
        //        estudiante.SetDate("createAt", DateTime.Now);
        //        var result = CreateDocument(estudiante);

        //        return (result != null ?
        //            new ResApp<EstudianteClass>()
        //            {
        //                result = true,
        //                type = "success",
        //                Data = datos,
        //                MSG = ResMSG.Estudiante.ESTUDIANTE_CREADO
        //            } :
        //            new ResApp<EstudianteClass>()
        //            {
        //                result = false,
        //                type = "error",
        //                Data = null,
        //                MSG = ResMSG.Estudiante.EXISTE_ESTUDIANTE
        //            }); 
        //    }

        //}
        //public ResApp<EstudianteClass> UpdateEstudiante(EstudianteClass estudiante)
        //{
        //    var mutableDict = new MutableDictionaryObject(estudiante.ToDictionary());

        //    using (var doc = database.GetDocument(estudiante.Cedula))
        //    {
        //        if(doc!= null)
        //        {
        //            var mutableDoc = doc.ToMutable();
        //            mutableDoc.SetData(mutableDict.ToDictionary());
        //            mutableDoc.SetDate("updateAt", DateTime.Now);
        //            var result = UpdateDocument(mutableDoc);
        //           return (result != null ?
        //            new ResApp<EstudianteClass>()
        //            {
        //                result = true,
        //                type = "success",
        //                Data = null,
        //                MSG = ResMSG.Estudiante.CAMBIOS_GUARDADOS
        //            } :
        //            new ResApp<EstudianteClass>()
        //            {
        //                result = false,
        //                type = "warning",
        //                Data = null,
        //                MSG = ResMSG.Estudiante.CAMBIOS_NO_GUARDADOS
        //            });
        //        }
        //        return new ResApp<EstudianteClass>()
        //        {
        //            result = false,
        //            type = "error",
        //            Data = null,
        //            MSG = ResMSG.Estudiante.ESTUDIANTE_NO_EXISTE
        //        };
                
        //    }
        //}
        //public ResApp<string> DeleteEstudiante(string Cedula)
        //{
             
            
        //    if (ExistEstudiante(Cedula))
        //    {
        //        var response = new ResApp<string>();
        //            var result= Deletedocument(Cedula);
        //        if (result)
        //        {
        //            response.result = true;
        //            response.type = "success";
        //            response.MSG = ResMSG.Estudiante.ESTUDIANTE_ELIMINADO;
        //        }
        //        else
        //        {
        //            response.result = false;
        //            response.type = "error";
        //            response.MSG = ResMSG.Estudiante.ESTUDIANTE_NO_ELIMINADO;
        //        }

        //        return response;
                    
        //    }
        //    return new ResApp<string>()
        //    {
        //        result = false,
        //        MSG = ResMSG.Estudiante.ESTUDIANTE_NO_EXISTE,
        //        type = "error",
        //        Data = null
        //    };



        //}

        //public EstudianteClass GetEstudiante(string Cedula)
        //{
        //    if (ExistEstudiante(Cedula))
        //    {
        //        return ConvertTo.ToClass<EstudianteClass>(database.GetDocument(Cedula).ToDictionary());
        //    }
        //    return null;
        //}
        //public List<EstudianteClass> GetAllEstudiantes()
        //{
        //    using (var query = QueryBuilder.Select(SelectResult.All())
        //                                    .From(DataSource.Database(database))
        //                                    .Where(Expression.Property("type").EqualTo(Expression.String("Estudiante"))))
        //    {
        //        var listEstudiantes = new List<EstudianteClass>();

        //        foreach(var estudianteData in query.Execute())
        //        {
        //            listEstudiantes.Add(ConvertTo.ToClass<EstudianteClass>(estudianteData.ToDictionary()));
        //        }
        //        return listEstudiantes;
        //    }
        //}

        // Falsta bulk Get estudiantes en List
        private void Main()
        {
            // Get the database (and create it if it doesn't exist)
            var database = new Database("mydb");
            // Create a new document (i.e. a record) in the database
            string id = null;
            using (var mutableDoc = new MutableDocument())
            {
                mutableDoc.SetFloat("version", 2.0f)
                    .SetString("type", "SDK");

                // Save it to the database
                database.Save(mutableDoc);
                id = mutableDoc.Id;
            }

            // Update a document
            using (var doc = database.GetDocument(id))
            using (var mutableDoc = doc.ToMutable())
            {
                mutableDoc.SetString("language", "C#");
                database.Save(mutableDoc);

                using (var docAgain = database.GetDocument(id))
                {
                    Console.WriteLine($"Document ID :: {docAgain.Id}");
                    Console.WriteLine($"Learning {docAgain.GetString("language")}");
                }
            }

            // Create a query to fetch documents of type SDK
            // i.e. SELECT * FROM database WHERE type = "SDK"
            using (var query = QueryBuilder.Select(SelectResult.All())
                .From(DataSource.Database(database))
                .Where(Expression.Property("type").EqualTo(Expression.String("SDK"))))
            {
                // Run the query
                var result = query.Execute();
                Console.WriteLine($"Number of rows :: {result.Count()}");
            }

            // Create replicator to push and pull changes to and from the cloud
            var targetEndpoint = new URLEndpoint(new Uri("ws://localhost:4000/db/getting-started-db"));
            var replConfig = new ReplicatorConfiguration(database, targetEndpoint)
            {

                // Add authentication
                Authenticator = new BasicAuthenticator("john", "pass")
            };

            // Create replicator (make sure to add an instance or static variable
            // named _Replicator)
            var _Replicator = new Replicator(replConfig);
            _Replicator.AddChangeListener((sender, args) =>
            {
                if (args.Status.Error != null)
                {
                    Console.WriteLine($"Error :: {args.Status.Error}");
                }
            });

            _Replicator.Start();
        }

    }

    public static class globalEstudiantesDbControl
    {
        private static Database databaseAll { get { return new Database("all_estudiantes", new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Estudiantes" }); } set { databaseAll = value; } }

        public static Document GetEstudiante(string Cedula)
        {
            if (ExistEstudiante(Cedula))
            {
                return databaseAll.GetDocument(Cedula);
            }
            return null;
        }
        //public static List<EstudianteClass> GetAllEstudiantes()
        //{
        //    using (var query = QueryBuilder.Select(SelectResult.All())
        //                                    .From(DataSource.Database(databaseAll))
        //                                    .Where(Expression.Property("type").EqualTo(Expression.String("Estudiante"))))
        //    {
        //        var listEstudiantes = new List<EstudianteClass>();

        //        foreach (var estudianteData in query.Execute())
        //        {
        //            listEstudiantes.Add(ConvertTo.ToClass<EstudianteClass>(estudianteData.ToDictionary()));
        //        }
        //        return listEstudiantes;
        //    }
        //}
        public static bool ExistEstudiante(string DocId)
        {
            return databaseAll.GetDocument(DocId) != null;
        }
    }
}

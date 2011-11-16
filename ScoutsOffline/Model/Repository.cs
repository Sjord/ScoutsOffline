using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Reflection;


namespace ScoutsOffline.Model
{
    public class Repository
    {
        private string BaseDirectory;
        private DataContractSerializer serializer;
        private string file;

        private StoredModel _model;
        public StoredModel Model
        {
            get
            {
                if (_model == null)
                {
                    _model = Get();
                }
                return _model;
            }
        }

        public Repository(string username)
        {
            this.BaseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.serializer = new DataContractSerializer(typeof(StoredModel));
            this.file = Path.Combine(BaseDirectory, username + ".xml");
        }

        public StoredModel Get()
        {
            if (!File.Exists(file))
            {
                return new StoredModel();
            }
            using (var stream = File.OpenRead(file))
            {
                return (StoredModel)serializer.ReadObject(stream);
            }
        }

        public void Store()
        {
            Store(Model);
        }

        public void Store(StoredModel model)
        {
            if (File.Exists(file))
            {
                var tmpFile = file + "~";
                StoreToFile(model, tmpFile);
                File.Replace(tmpFile, file, file + ".bkp");
            }
            else
            {
                StoreToFile(model, file);
            }
        }

        private void StoreToFile(StoredModel model, string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (var xmlWriter = XmlWriter.Create(filename, settings))
            {
                serializer.WriteObject(xmlWriter, model);
            }
        }
    }
}

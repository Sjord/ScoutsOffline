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
    public class Repository<T>
    {
        private DataContractSerializer serializer;
        private string file;

        private T _model;
        public T Model
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

        public Repository(string file)
        {
            this.serializer = new DataContractSerializer(typeof(T));
            this.file = file;
        }

        public T Get()
        {
            if (!File.Exists(file))
            {
                return default(T);
            }
            using (var stream = File.OpenRead(file))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        public void Store()
        {
            Store(Model);
        }

        public void Store(T model)
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

        private void StoreToFile(T model, string filename)
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

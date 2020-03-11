using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ParadoxFramework.IO
{
    public static class ParadoxSerialization
    {
        /// <summary>
        /// Guarda la información de un objeto en un archivo, en formato json o binario.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSave">Objeto a guardar.</param>
        /// <param name="path">Ubicación del archivo.</param>
        /// <param name="encripted">Si será encriptado en binario o en json.</param>
        public static void SaveData<T>(this T objectToSave, string path, bool encripted)
        {
            if (encripted)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream binaryFile = File.Create(path);
                bf.Serialize(binaryFile, objectToSave);
                binaryFile.Close();
                return;
            }

            StreamWriter jsonFile = File.CreateText(path);
            string json = JsonUtility.ToJson(objectToSave, true);
            jsonFile.Write(json);
            jsonFile.Close();
        }

        public static void CreateFolder(string path, string folderName)
        {
            if (!Directory.Exists(path))
                return;

            if (Directory.Exists(path + "/" + folderName))
                return;

            Directory.CreateDirectory(path + "/" + folderName);
        }

        /// <summary>
        /// Cara la información de un objeto desde un archivo anteriormente serializado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectLoad">Objeto a cargar.</param>
        /// <param name="path">Ubicación del archivo.</param>
        /// <returns>Objeto cargado.</returns>
        public static T LoadData<T>(this T objectLoad, string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log($"{objectLoad}: El objeto no pudo ser cargado ya que no existe en la ubicación dada {path}");
                return default;
            }

            if (path.Contains("json"))
            {
                string fileJson = File.ReadAllText(path);
                return JsonUtility.FromJson<T>(fileJson);
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileBinary = File.Open(path, FileMode.Open);
            T data = (T)bf.Deserialize(fileBinary);
            fileBinary.Close();

            return data;
        }

        public static bool CheckFile(string path) => File.Exists(path);

        public static bool CheckFolder(string path) => Directory.Exists(path);
    }
}
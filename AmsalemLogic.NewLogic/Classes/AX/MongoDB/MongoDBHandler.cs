using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using AmsalemLogic.VBClasses;
using System.Drawing;

namespace AmsalemLogic.NewLogic.Classes.Products.ArchiveMongoDB
{
    public class MongoDBHandler
    {

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public bool IsImageExist(string imageName)
        {
            MongoClient client = connection();
            var db = client.GetDatabase("CreditCardIMG");
            var bucket = new GridFSBucket(db);
            var filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, imageName));
            var options = new GridFSFindOptions
            {
                Limit = 1
            };

            using (var cursor = bucket.Find(filter, options))
            {

                var fileInfo = cursor.ToList().FirstOrDefault();
                if (fileInfo == null)
                    return false;
                else
                    return true;     
                     
            }
        }

        public MongoClient connection()
        {
            MongoClient mongoclient = new MongoClient("mongodb://localhost/local");
            return mongoclient;
        }

        public void UploadImage(Image imageToUpload, string imageName)
        {

            var cardHashName = imageName.Substring(0, 4) +
                               imageName.Substring(imageName.Length - 8, 4) +
                               imageName.Substring(imageName.Length - 4, 4);

            MongoClient client = connection();
            var db = client.GetDatabase("CreditCardIMG");
            var bucket = new GridFSBucket(db);
            Image img = imageToUpload;
            byte[] source = ImageToByteArray(img);

            if(IsImageExist(cardHashName))
            {
                var filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, cardHashName));
                var options = new GridFSFindOptions
                {
                    Limit = 1
                };

                using (var cursor = bucket.Find(filter, options))
                {
                    var fileInfo = cursor.ToList().FirstOrDefault();
                    bucket.Delete(fileInfo.Id);
                    bucket.UploadFromBytes(cardHashName, source);
                }

            }
            else
                bucket.UploadFromBytes(cardHashName, source);

        }

        public byte[] ReadImage(string cardNumber)
        {

            //var cardHashName = cardNumber.Substring(0, 4) +
            //                    cardNumber.Substring(cardNumber.Length - 8, 4) +
            //                    cardNumber.Substring(cardNumber.Length - 4, 4);
            var  cardHashName = cardNumber;

            MongoClient client = connection();
                var db = client.GetDatabase("CreditCardIMG");
                var bucket = new GridFSBucket(db);
                var filter = Builders<GridFSFileInfo>.Filter.And(
                    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, cardHashName));
                var options = new GridFSFindOptions
                {
                    Limit = 1
                };
                using (var cursor = bucket.Find(filter, options))
                {
                    var fileInfo = cursor.ToList().FirstOrDefault();
                    if (fileInfo == null)
                    {
                        return ReadImage("000000000000"); //Image Not Available.
                    }
                    else
                    {
                        var imageToReturn = bucket.DownloadAsBytes(fileInfo.Id);
                        return imageToReturn;
                    }
                }


        }  

    }



}


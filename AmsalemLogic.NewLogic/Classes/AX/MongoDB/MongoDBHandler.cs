using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using AmsalemLogic.VBClasses;

namespace AmsalemLogic.NewLogic.Classes.Products.ArchiveMongoDB
{
    public class MongoDBHandler
    {
        public ResultOfOperation MongoCreateCollection()
        {
            var databaseName = MongoUrl.Create("mongodb://AMS-FS:27017/DB").DatabaseName;
            return null;
        }
        public MongoClient connection()
        {
            MongoClient mongoclient = new MongoClient("mongodb://AMS-FS:27017/DB");
            return mongoclient;
        }

      

            //TODO 
        //private ResultOfOperation UploadFile()
        //{
            //MongoClient client = new MongoClient("mongodb://AMS-FS:27017");
            //MongoServer server = client.GetServer();
            //MongoDatabase db = server.GetDatabase("DB");
            //MongoCollection collection = db.GetCollection("ArchiveFiles");

            //Stream memoryStream = new MemoryStream(ArchiveDocuments.FileData);
            //MongoGridFSFileInfo gfsi = db.GridFS.Upload(memoryStream, ArchiveDocuments.FileName);
            //MongoGridFSSettings gridFsSettings = new MongoGridFSSettings();
            //gridFsSettings.ChunkSize = 1024;
            //BsonDocument FileMetadata = new BsonDocument
            //                        { 
            //                                {"FileName", ArchiveDocuments.FileName.Trim()},    
            //                                {"FileId",gfsi.Id.AsObjectId},
            //                                {"UploadDate", ArchiveDocuments.UploadDate},
            //                                {"UploadedByClockid", ArchiveDocuments.UploadedByClockid }
            //                        };
            //db.GridFS.SetMetadata(gfsi, FileMetadata);
            //ArchiveDocuments.FileId = gfsi.Id.AsObjectId;
            //var ss = collection.Insert(ArchiveDocuments);
            //return new ResultOfOperation(ss.Response.GetElement("ok").Value.ToBoolean());
       // }

        //public List<ArchiveDocument> SearchDocument(string AxCompany, string ReferenceNumber, DocumentType Type)
        //{
        //    var mongoclient = connection();
        //    var db = mongoclient.GetDatabase("DB");
        //    var collection = db.GetCollection<BsonDocument>("ArchiveFiles");
        //    var filter = Builders<BsonDocument>.Filter.Eq("ReferenceNumber2", ReferenceNumber.Trim()) & Builders<BsonDocument>.Filter.Eq("Type", Type);
        //    if (!string.IsNullOrEmpty(AxCompany))
        //    {
        //        filter &= Builders<BsonDocument>.Filter.Eq("ReferenceNumber", AxCompany.Trim());
        //    }

        //    var result = collection.Find(filter).ToList();
        //    var ListOfArchiveDocument = new List<ArchiveDocument>();

        //    foreach (var item in result)
        //    {

        //        var ArchiveDocumentObj = ParseSingleResult(item);
        //        ListOfArchiveDocument.Add(ArchiveDocumentObj);
        //    }
        //    return ListOfArchiveDocument;
        //}

        //private ArchiveDocument ParseSingleResult(BsonDocument item)
        //{
        //    var ArchiveDocumentObj = new ArchiveDocument();
        //    if (BsonNull.Value == item.GetElement("Description").Value)
        //    {
        //        ArchiveDocumentObj.Description = "";
        //    }
        //    else
        //    {
        //        ArchiveDocumentObj.Description = HttpUtility.UrlDecode((string)item.GetElement("Description").Value);
        //    }
        //    ArchiveDocumentObj.FileName = HttpUtility.UrlDecode((string)item.GetElement("FileName").Value);
        //    ArchiveDocumentObj.UploadDate = (DateTime)item.GetElement("UploadDate").Value;
        //    if (BsonNull.Value == item.GetElement("ReferenceNumber").Value)
        //    {
        //        ArchiveDocumentObj.ReferenceNumber = "";
        //    }
        //    else
        //    {
        //        ArchiveDocumentObj.ReferenceNumber = (string)item.GetElement("ReferenceNumber").Value;
        //    }
        //    ArchiveDocumentObj.ReferenceNumber2 = (string)item.GetElement("ReferenceNumber2").Value;
        //    ArchiveDocumentObj.Type = (DocumentType)(int)item.GetElement("Type").Value;
        //    ArchiveDocumentObj._id = (Guid)item.GetElement("_id").Value;
        //    var element = item.GetElement("mimeType");
        //    if (element != null)
        //    {
        //        ArchiveDocumentObj.mimeType = (string)element.Value;
        //    }
        //    ArchiveDocumentObj.FileId = (ObjectId)item.GetElement("FileId").Value;
        //    ArchiveDocumentObj.UploadedByClockid = (int)item.GetElement("UploadedByClockid").Value;
        //    var user = ClassUsers.GetUserByWorkerID(ArchiveDocumentObj.UploadedByClockid, false);
        //    if (user != null)
        //    {
        //        ArchiveDocumentObj.UploadedByName = user.AgentName;
        //    }
        //    return ArchiveDocumentObj;
        //}

        //public ArchiveDocument SearchFileDocument(Guid id)
        //{
        //    var mongoclient = connection();
        //    var db = mongoclient.GetDatabase("DB");
        //    var collection = db.GetCollection<BsonDocument>("ArchiveFiles");
        //    var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
        //    var item = collection.Find(filter).FirstOrDefault();
        //    var ArchiveDocumentObj = this.ParseSingleResult(item);
        //    ArchiveDocumentObj.FileData = ReadFile(ArchiveDocumentObj);
        //    return ArchiveDocumentObj;
        //}

        //public byte[] ReadFile(ArchiveDocument ArchiveDocumentObj)
        //{
        //    MongoClient client = new MongoClient("mongodb://AMS-FS:27017");
        //    MongoServer server = client.GetServer();
        //    MongoDatabase db = server.GetDatabase("DB");
        //    MongoCollection collection = db.GetCollection("ArchiveFiles");

        //    var query = Query<ArchiveDocument>.EQ(e => e.FileId, ArchiveDocumentObj.FileId);
        //    var entity = db.GridFS.FindOne(query);

        //    MongoGridFSFileInfo file1 = db.GridFS.FindOne(Query.EQ("_id", ArchiveDocumentObj.FileId));
        //    using (var stream = file1.OpenRead())
        //    {
        //        var bytes = new byte[stream.Length];
        //        stream.Read(bytes, 0, (int)stream.Length);
        //        ArchiveDocumentObj.FileData = bytes;
        //    }
        //    return ArchiveDocumentObj.FileData;
        //}

        //public ResultOfOperation DeleteCollection()
        //{
        //    MongoClient client = new MongoClient("mongodb://AMS-FS:27017");
        //    MongoServer server = client.GetServer();
        //    MongoDatabase db = server.GetDatabase("DB");
        //    MongoCollection collection = db.GetCollection("ArchiveFiles");
        //    MongoCollection<ArchiveDocument> ArchiveFiles = db.GetCollection<ArchiveDocument>("ArchiveFiles");
        //    ArchiveFiles.Drop();
        //    return null;
        //}
        //public ResultOfOperation RemoveItem(string ReferenceNumber, DocumentType Type)
        //{
        //    var mongoclient = connection();
        //    var db = mongoclient.GetDatabase("DB");
        //    var collection = db.GetCollection<BsonDocument>("ArchiveFiles");
        //    var filter = Builders<BsonDocument>.Filter.Eq("ReferenceNumber2", ReferenceNumber.Trim()) & Builders<BsonDocument>.Filter.Eq("Type", Type);
        //    var result = collection.Find(filter).ToList();
        //    var ListOfArchiveDocument = new List<ArchiveDocument>();
        //    collection.DeleteMany(filter);
        //    return null;
        //}
    }
}

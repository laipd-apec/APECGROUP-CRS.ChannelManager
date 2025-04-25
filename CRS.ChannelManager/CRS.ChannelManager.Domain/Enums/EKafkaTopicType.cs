using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Enums
{
    public static class EKafkaTopicType
    {
        public const string AllotmentRoomAvailability = "allotment-room-availability";
        public const string BookingResult = "booking-result";
        public const string BookingUpdateInventory = "booking-update-inventory";
        public const string SyncMasterData = "sync-master-data";
    }

    public static class EKafkaActionType
    {
        public const string Create = "create";
        public const string Update = "update";
        public const string Delete = "delete";
        public const string Remove = "remove";
        public const string UpdateStatus = "update-status";
    }

    public static class EKafkaMethodType
    {
        public const string Insert = "Insert";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string Remove = "Remove";
        public const string FindById = "FindById";
        public const string FindByCode = "FindByCode";
        public const string FindBySyncKey = "FindBySyncKey";
        public const string FindByPropertySyncKey = "FindObjectByPropertyValue";
        public const string SaveChange = "SaveChange";
        public const string Commit = "Commit";
    }

    public static class EKafkaObjectType
    {
        public const string NameGetId = "Id";
        public const string NameDeleted = "Deleted";
        public const string NameDeletedDate = "DeletedDate";
        public const string NameGetResultTask = "Result";
        public const string NameGetSyncKeyEntity = "SyncKey";
        public const string NameMapJson = "MapJsonToEntity";
        public const string NameGetValueFromJson = "GetValueFromJson";
        public const string KeyEndObject = "Entity";
        public const string NameGetObject = "Data";
        public const string KeyEndRepository = "Repository";
        public const string FolderRepository = "Infrastructure.Repositories";
    }
}

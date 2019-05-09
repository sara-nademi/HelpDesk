using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Infra.Common;

namespace Helpdesk.BLL.Convert
{
    public class LocationImport : IDataImport
    {
        // The last of import location
        private readonly HRMWFEntities _db;

        private readonly long _insertCount;

        public long InsertedCount
        {
            get
            {
                return _insertCount;
            }
        }

        private readonly long _updateCount;

        public long UpdateCount
        {
            get
            {
                return _updateCount;
            }
        }

        private long _deleteCount;

        public long DeleteCount
        {
            get
            {
                return _deleteCount;
            }
        }

        public LocationImport()
        {
            _db = new HRMWFEntities();
            _insertCount = 0;
            _updateCount = 0;
            _deleteCount = 0;
        }

        public void Import(DataSet ds)
        {
            var allRecords = _db.Location.ToList();
            allRecords.ForEach(p =>
                                   {
                                       p.IsDeleted = true;
                                   });
            _db.SaveChanges();

            #region add building

            var buildingDic = new Dictionary<string, Location>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var name = row["Name"].ToString();
                var bldCode = row["BldCode"].ToString();

                var building = _db.Location.FirstOrDefault(p => p.Code == bldCode);

                if (buildingDic.ContainsKey(bldCode))
                    continue;

                if (building != null)
                {
                    building.Title = name;
                    building.ParentLocationID = null;
                    building.IsDeleted = false;
                    _db.Location.ApplyCurrentValues(building);

                    buildingDic.Add(bldCode, building);
                }
                else
                {
                    var buildingLocation = new Location
                                               {
                                                   Code = bldCode,
                                                   ParentLocationID = null,
                                                   Title = name,
                                                   IsDeleted = false,
                                                   IsActive = true,
                                                   InsertDate = DateTime.Now,
                                                   InsertUser = "admin"
                                               };
                    _db.Location.AddObject(buildingLocation);
                    buildingDic.Add(bldCode, buildingLocation);
                }
            }
            _db.SaveChanges();

            #endregion

            #region add floors

            var floorDic = new Dictionary<string, Location>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var floorName = row["FloorName"].ToString();
                var floorCode = row["FloorCode"].ToString();
                var buldingCode = row["BldCode"].ToString();

                var floor = _db.Location.FirstOrDefault(p => p.Code == floorCode);

                if (floorDic.ContainsKey(floorCode))
                    continue;

                var buildingParent = buildingDic.FirstOrDefault(p => p.Key == buldingCode);

                if (floor != null)
                {
                    floor.Title = floorName;
                    floor.IsDeleted = false;
                    floor.ParentLocationID = buildingParent.Value.LocationID;
                    _db.Location.ApplyCurrentValues(floor);

                    floorDic.Add(floorCode, floor);
                }
                else
                {
                    var floorLocation = new Location
                                            {
                                                ParentLocationID = buildingParent.Value.LocationID,
                                                Code = floorCode,
                                                Title = floorName,
                                                IsDeleted = false,
                                                IsActive = true,
                                                InsertDate = DateTime.Now,
                                                InsertUser = "admin"
                                            };
                    _db.Location.AddObject(floorLocation);

                    floorDic.Add(floorCode, floorLocation);
                }
            }
            _db.SaveChanges();

            #endregion

            #region add rooms

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var roomDesc = row["RoomDscp"].ToString();
                var roomNo = Regex.Replace(roomDesc, "[^0-9]", "");

                var roomName = roomDesc;
                if (!string.IsNullOrEmpty(roomNo))
                    roomName = string.Format("اتاق {0}", roomNo);

                var roomCode = row["BldFloorRoomCode"].ToString();

                var floorCode = row["FloorCode"].ToString();

                var room = _db.Location.FirstOrDefault(p => p.Code == roomCode);

                var floorParent = floorDic.FirstOrDefault(p => p.Key == floorCode);

                if (room != null)
                {
                    room.Title = roomName;
                    room.IsDeleted = false;
                    room.ParentLocationID = floorParent.Value.LocationID;
                    _db.Location.ApplyCurrentValues(room);
                }
                else
                {
                    var floorLocation = new Location
                                            {
                                                ParentLocationID = floorParent.Value.LocationID,
                                                Code = roomCode,
                                                Title = roomName,
                                                IsDeleted = false,
                                                IsActive = true,
                                                InsertDate = DateTime.Now,
                                                InsertUser = "admin"
                                            };
                    _db.Location.AddObject(floorLocation);
                }
            }
            _db.SaveChanges();

            #endregion

            var deleteObject = _db.Location.Where(p => p.IsDeleted);
            _deleteCount = deleteObject.Count();
        }

        //public void Import(DataSet ds)
        //{
        //    var allRecords = _db.Location.Where(p => p.IsDeleted || !p.IsDeleted).ToList();
        //    allRecords.ForEach(p => p.IsDeleted = true);
        //    _db.SaveChanges();

        //    var parentDic = new Dictionary<string, Location>();
        //    var tabagheDic = new Dictionary<string, Location>();

        //    foreach (DataRow parentRow in ds.Tables[0].Rows)
        //    {
        //        var name = parentRow["Name"].ToString();
        //        var bldCode = parentRow["BldCode"].ToString();

        //        var loc = _db.Location.FirstOrDefault(p => p.Code == bldCode);
        //        if (loc != null)
        //        {
        //            loc.Title = name;
        //            loc.IsDeleted = false;
        //            _db.Location.ApplyCurrentValues(loc);
        //            _updateCount++;
        //            if (!parentDic.Keys.Contains(loc.Title))
        //                parentDic.Add(loc.Title, loc);
        //        }
        //        if (parentDic.Keys.Contains(name)) continue;

        //        var q = _db.Location.LastOrDefault(p => p.ParentLocationID == null);
        //        long max = q.LocationID;
        //        max++;
        //        var plocation = new Location
        //        {
        //            LocationID = max,
        //            Code = parentRow["BldCode"].ToString(),
        //            Title = name,
        //            IsDeleted = false,
        //            IsActive = true,
        //            InsertDate = DateTime.Now,
        //            InsertUser = "admin"
        //        };

        //        _db.Location.AddObject(plocation);
        //        _insertCount++;
        //        parentDic.Add(name, plocation);
        //    }

        //    _db.SaveChanges();

        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        var locationID = long.Parse(row["RoomTotalCode"].ToString());
        //        var parentName = row["Name"].ToString();
        //        var fname = row["FloorName"].ToString();
        //        var loc2 = _db.Location.FirstOrDefault(r => r.LocationID == locationID);
        //        if (loc2 != null)
        //        {
        //            loc2.Title = row["FloorName"].ToString();
        //            loc2.IsDeleted = false;
        //            _db.Location.ApplyCurrentValues(loc2);
        //            _updateCount++;
        //            if (!tabagheDic.Keys.Contains(loc2.Title))
        //                tabagheDic.Add(loc2.Title, loc2);
        //        }
        //        if (tabagheDic.Keys.Contains(row["FloorName"].ToString())) continue;

        //        var name1 = row["Name"].ToString();
        //        var parent = parentDic.FirstOrDefault(p => p.Key == name1);
        //        var tabaghelocation = new Location
        //        {
        //            ParentLocationID = parent.Value.LocationID,
        //            Title = row["FloorName"].ToString(),
        //            IsDeleted = false,
        //            IsActive = true,
        //            InsertUser = "admin",
        //            InsertDate = DateTime.Now,
        //        };
        //        _db.Location.AddObject(tabaghelocation);
        //        _insertCount++;
        //        tabagheDic.Add(row["FloorName"].ToString(), tabaghelocation);
        //    }
        //    _db.SaveChanges();

        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        var locationID = long.Parse(row["RoomTotalCode"].ToString());
        //        var fname2 = row["FloorName"].ToString();
        //        var rc = row["RoomCode"].ToString();
        //        var tabaghe = tabagheDic.FirstOrDefault(p => p.Key == fname2);
        //        var room = _db.Location.FirstOrDefault(r => r.LocationID == locationID);

        //        if (room != null)
        //        {
        //            room.Title = "اتاق " + row["RoomCode"];
        //            room.IsDeleted = false;
        //            room.InsertDate = DateTime.Now;
        //            room.InsertUser = "admin";
        //            _db.Location.ApplyCurrentValues(room);
        //            _db.SaveChanges();
        //            _updateCount++;
        //            continue;
        //        }
        //        var otaghLocation = new Location
        //        {
        //            LocationID = locationID,
        //            ParentLocationID = tabaghe.Value.LocationID,
        //            Title = "اتاق " + row["RoomCode"],
        //            IsDeleted = false,
        //            InsertDate = DateTime.Now,
        //            InsertUser = "admin",
        //            IsActive = true
        //        };
        //        _insertCount++;
        //        _db.Location.AddObject(otaghLocation);
        //        _db.SaveChanges();
        //    }

        //    var deleteObject = _db.Location.Where(p => p.IsDeleted);
        //    _deleteCount = deleteObject.Count();
        //}

        public void Import(string xmlDirectory)
        {
            var dsLocation = new DataSet("GetPhysicalLocation");
            dsLocation.ReadXml(xmlDirectory);
            Import(dsLocation);
        }
    }
}
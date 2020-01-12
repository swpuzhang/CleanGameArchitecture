var url = "mongodb://localhost:27017/SkyWatch";
var db = connect(url);
db.UserIdGenInfo.save({"_id":NumberLong(10000000000),"UserId":NumberLong(10000000000)})
db.createCollection("AccountInfo");
db.AccountInfo.ensureIndex({PlatformAccount:1}, {unique: true});
db.createCollection("LevelConfig");
db.LevelConfig.ensureIndex({Level:1}, {unique: true});
db.LevelConfig.insert( {Level:1, NeedExp:10});
db.LevelConfig.insert( {Level:2, NeedExp:10});
db.CoinsRangeConfig.insert({CoinsBegin:NumberLong(5000),CoinsEnd:NumberLong(100000),Blind:NumberLong(500)})
db.CoinsRangeConfig.insert({CoinsBegin:NumberLong(100000),CoinsEnd:NumberLong(-1),Blind:NumberLong(2000)})
db.RoomListConfig.ensureIndex({Blind:1}, {unique: true});
db.RoomListConfig.insert({RoomType:1, Blind:NumberLong(700), MinCoins:NumberLong(3500), MaxCoins:NumberLong(10000), TipsPersent:NumberLong(10),MinCarry:NumberLong(3500), MaxCarry:NumberLong(35000)})
db.RoomListConfig.insert({RoomType:1, Blind:NumberLong(1500), MinCoins:NumberLong(7500), MaxCoins:NumberLong(-1), TipsPersent:NumberLong(20),MinCarry:NumberLong(7500), MaxCarry:NumberLong(75000)})
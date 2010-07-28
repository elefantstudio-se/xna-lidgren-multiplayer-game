//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Shared;

//namespace Client
//{
//    class Drawable
//    {
//        public int ID { get; set; }

//        public void Draw() { }
//    }

//    internal interface IData
//    {
//        int ID { get; set; }
//    }

//    class PlayerData : IData
//    {
//        public int ID { get; set; }
//        public string Name { get; set; }
//    }

//    class EnemyData : IData
//    {
//        public int ID { get; set; }
//        public int Damage { get; set; }
//    }

//    interface IUpdateable<T> where T : IData
//    {
//        void Update(T data);
//    }

//    class Enemy : Drawable, IUpdateable<EnemyData>
//    {
//        public void Update(EnemyData data) { }
//    }

//    class Player : Drawable, IUpdateable<PlayerData>
//    {
//        public void Update(PlayerData data) {}
//    }

//    class DataHolder
//    {
//        public IUpdateable<IData> Entity{ get; set;}
//        public IData Data{ get; set;}

//        public DataHolder(IUpdateable<IData> entity, IData data)
//        {
//            Entity = entity;
//            Data = data;
//        }
//    }

//    class Dev
//    {
//        public Dev()
//        {
//            var p1 = new Player();
//            var p1Data = new PlayerData();
//            var e1 = new Enemy();
//            var e1Data = new EnemyData();

//            var playerData = new DataHolder((IUpdateable<IData>)p1, p1Data);
//            playerData.Entity.Update(playerData.Data);
//            var enemyData = new DataHolder<Enemy, EnemyData>(e1, e1Data);

//            Dictionary<int, DataHolder> list = new Dictionary<int, DataHolder>();
//            list.Add(p1.ID, playerData);
//            list.Add(e1.ID, enemyData);

//            foreach (var value in list.Values)
//            {
//                var entity = value.Entity;
//                entity.Update(value.Data);
//            }
//        }
//    }
//}

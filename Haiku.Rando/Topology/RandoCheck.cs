﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Haiku.Rando.Topology
{
    public sealed class RandoCheck : IRandoNode
    {
        public RandoCheck(CheckType type, int sceneId, Vector2 position, int checkId)
        {
            Type = type;
            SceneId = sceneId;
            Position = position;
            CheckId = checkId;
        }

        public string Name => $"{Type}[{CheckId}]";

        public CheckType Type { get; set; }

        public Vector2 Position { get; set; }

        public int SceneId { get; set; }

        public int CheckId { get; set; }

        public int SaveId { get; set; }

        public bool IsShopItem { get; set; }

        public string Alias { get; set; }

        public string GetAlias(int sceneId) => Alias;

        public List<InRoomEdge> Incoming { get; set; } = new List<InRoomEdge>();

        IReadOnlyList<IRandoEdge> IRandoNode.Incoming => Incoming;

        IReadOnlyList<IRandoEdge> IRandoNode.Outgoing => Array.Empty<IRandoEdge>();
        
    }
}

﻿using LiteDB;
using Newtonsoft.Json;
using System;

namespace NTMiner.Core.Kernels.Impl {
    public class KernelData : IKernel, IDbEntity<Guid> {
        public KernelData() {
        }

        public Guid GetId() {
            return this.Id;
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public string FullName {
            get {
                return $"{this.Code}{this.Version}";
            }
        }
        public ulong PublishOn { get; set; }

        public string Package { get; set; }

        public string PackageHistory { get; set; }

        public string Sha1 { get; set; }

        public long Size { get; set; }

        public int SortNumber { get; set; }

        public PublishStatus PublishState { get; set; }

        public string HelpArg { get; set; }

        public string Notice { get; set; }

        public Guid KernelInputId { get; set; }
        public Guid KernelOutputId { get; set; }
    }
}

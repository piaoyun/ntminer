﻿using System;

namespace NTMiner.Core.Kernels {
    public interface IKernelOutputTranslater : IEntity<Guid> {
        Guid KernelOutputId { get; }
        string RegexPattern { get; }
        string Replacement { get; }
        string Color { get; }
        int SortNumber { get; }
        bool IsPre { get; }
    }
}

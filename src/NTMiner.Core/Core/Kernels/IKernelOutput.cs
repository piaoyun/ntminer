﻿using System;

namespace NTMiner.Core.Kernels {
    public interface IKernelOutput : IEntity<Guid> {
        string Name { get; }
        string TotalSpeedPattern { get; }
        string TotalSharePattern { get; }
        string AcceptSharePattern { get; }
        string RejectSharePattern { get; }
        string RejectPercentPattern { get; }
        string GpuSpeedPattern { get; }

        string DualTotalSpeedPattern { get; }
        string DualTotalSharePattern { get; }
        string DualAcceptSharePattern { get; }
        string DualRejectSharePattern { get; }
        string DualRejectPercentPattern { get; }
        string DualGpuSpeedPattern { get; }
    }
}

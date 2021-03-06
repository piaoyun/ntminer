﻿using NTMiner.ServiceContracts.DataObjects;
using System;
using System.Collections.Generic;

namespace NTMiner.Data {
    public interface ICoinSnapshotSet {
        List<CoinSnapshotData> GetLatestSnapshots(
            int limit,
            List<string> coinCodes, 
            out int totalMiningCount, 
            out int totalOnlineCount);
    }
}

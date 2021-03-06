﻿using NTMiner.ServiceContracts.DataObjects;
using System;
using System.ServiceModel;

namespace NTMiner.ServiceContracts {
    [ServiceContract]
    public interface IReportService : IDisposable {
        [OperationContract(IsOneWay = true)]
        void Login(LoginData message);

        [OperationContract(IsOneWay = true)]
        void ReportState(Guid clientId, bool isMining);

        [OperationContract(IsOneWay = true)]
        void ReportSpeed(SpeedData message);
    }
}

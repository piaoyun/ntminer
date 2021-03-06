﻿using NTMiner.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTMiner.Vms {
    public class WalletViewModels : ViewModelBase {
        public static readonly WalletViewModels Current = new WalletViewModels();
        private readonly Dictionary<Guid, WalletViewModel> _dicById = new Dictionary<Guid, WalletViewModel>();
        private WalletViewModels() {
            Global.Access<WalletAddedEvent>(
                Guid.Parse("a0a78584-1c43-4364-b8f1-315c9de9e0bf"),
                "添加了钱包后调整VM内存",
                LogEnum.Log,
                action: (message) => {
                    _dicById.Add(message.Source.GetId(), new WalletViewModel(message.Source));
                    OnPropertyChanged(nameof(WalletList));
                    CoinViewModel coin;
                    if (CoinViewModels.Current.TryGetCoinVm(message.Source.CoinId, out coin)) {
                        coin.OnPropertyChanged(nameof(CoinViewModel.Wallets));
                        coin.CoinKernel?.CoinKernelProfile?.SelectedDualCoin?.OnPropertyChanged(nameof(CoinViewModel.Wallets));
                    }
                });
            Global.Access<WalletRemovedEvent>(
                Guid.Parse("126829e4-d78e-492f-a74d-d2d05aef0bc2"),
                "删除了钱包后调整VM内存",
                LogEnum.Log,
                action: (message) => {
                    _dicById.Remove(message.Source.GetId());
                    OnPropertyChanged(nameof(WalletList));
                    CoinViewModel coin;
                    if (CoinViewModels.Current.TryGetCoinVm(message.Source.CoinId, out coin)) {
                        coin.OnPropertyChanged(nameof(CoinViewModel.Wallets));
                        coin.CoinProfile?.OnPropertyChanged(nameof(CoinProfileViewModel.SelectedWallet));
                        coin.CoinKernel?.CoinKernelProfile?.SelectedDualCoin?.OnPropertyChanged(nameof(CoinViewModel.Wallets));
                    }
                });
            Global.Access<WalletUpdatedEvent>(
                Guid.Parse("718c3a5f-5059-452a-94d5-9714e81b1986"),
                "更新了钱包后调整VM内存",
                LogEnum.Log,
                action: (message) => {
                    _dicById[message.Source.GetId()].Update(message.Source);
                });
            foreach (var item in NTMinerRoot.Current.WalletSet) {
                _dicById.Add(item.GetId(), new WalletViewModel(item));
            }
        }

        public List<WalletViewModel> WalletList {
            get {
                return _dicById.Values.ToList();
            }
        }
    }
}

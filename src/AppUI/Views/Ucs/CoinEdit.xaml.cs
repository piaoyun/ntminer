﻿using NTMiner.Vms;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class CoinEdit : UserControl {
        public static void ShowEditWindow(CoinViewModel source) {
            ContainerWindow.ShowWindow(new ContainerWindowViewModel {
                IsDialogWindow = true,
                CloseVisible = System.Windows.Visibility.Visible,
                IconName = "Icon_Coin",
            }, ucFactory: (window) =>
            {
                CoinViewModel vm = new CoinViewModel(source);
                vm.CloseWindow = () => window.Close();
                return new CoinEdit(vm);
            }, fixedSize: true);
        }

        private CoinViewModel Vm {
            get {
                return (CoinViewModel)this.DataContext;
            }
        }
        public CoinEdit(CoinViewModel vm) {
            this.DataContext = vm;
            InitializeComponent();
            ResourceDictionarySet.Instance.FillResourceDic(this, this.Resources);
        }
    }
}

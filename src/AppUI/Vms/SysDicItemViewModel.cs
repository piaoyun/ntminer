﻿using NTMiner.Core;
using NTMiner.Core.SysDics;
using NTMiner.Views;
using NTMiner.Views.Ucs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NTMiner.Vms {
    public class SysDicItemViewModel : ViewModelBase, ISysDicItem {
        public static readonly SysDicItemViewModel PleaseSelect = new SysDicItemViewModel(Guid.Empty) {
            _code = string.Empty,
            _value = "请选择",
            _description = "请选择",
            _sortNumber = -1
        };

        private Guid _id;
        private Guid _dicId;
        private string _code;
        private string _value;
        private string _description;
        private int _sortNumber;
        public ICommand Remove { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand SortUp { get; private set; }
        public ICommand SortDown { get; private set; }
        public ICommand Save { get; private set; }

        public Action CloseWindow { get; set; }

        public Guid GetId() {
            return this.Id;
        }

        public SysDicItemViewModel(ISysDicItem data) : this(data.GetId()) {
            _dicId = data.DicId;
            _code = data.Code;
            _value = data.Value;
            _description = data.Description;
            _sortNumber = data.SortNumber;
        }

        public SysDicItemViewModel(Guid id) {
            _id = id;
            this.Save = new DelegateCommand(() => {
                if (NTMinerRoot.Current.SysDicItemSet.ContainsKey(this.Id)) {
                    Global.Execute(new UpdateSysDicItemCommand(this));
                }
                else {
                    Global.Execute(new AddSysDicItemCommand(this));
                }
                CloseWindow?.Invoke();
            });
            this.Edit = new DelegateCommand(() => {
                SysDicItemEdit.ShowEditWindow(this);
            });
            this.Remove = new DelegateCommand(() => {
                if (this.Id == Guid.Empty) {
                    return;
                }
                DialogWindow.ShowDialog(message: $"您确定删除{this.Code}系统字典项吗？", title: "确认", onYes: () => {
                    Global.Execute(new RemoveSysDicItemCommand(this.Id));
                }, icon: "Icon_Confirm");
            });
            this.SortUp = new DelegateCommand(() => {
                SysDicItemViewModel upOne = SysDicItemViewModels.Current.List.OrderByDescending(a => a.SortNumber).FirstOrDefault(a => a.SortNumber < this.SortNumber);
                if (upOne != null) {
                    int sortNumber = upOne.SortNumber;
                    upOne.SortNumber = this.SortNumber;
                    Global.Execute(new UpdateSysDicItemCommand(upOne));
                    this.SortNumber = sortNumber;
                    Global.Execute(new UpdateSysDicItemCommand(this));
                    SysDicViewModel sysDicVm;
                    if (SysDicViewModels.Current.TryGetSysDicVm(this.DicId, out sysDicVm)) {
                        sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItems));
                        sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItemsSelect));
                    }
                }
            });
            this.SortDown = new DelegateCommand(() => {
                SysDicItemViewModel nextOne = SysDicItemViewModels.Current.List.OrderBy(a => a.SortNumber).FirstOrDefault(a => a.SortNumber > this.SortNumber);
                if (nextOne != null) {
                    int sortNumber = nextOne.SortNumber;
                    nextOne.SortNumber = this.SortNumber;
                    Global.Execute(new UpdateSysDicItemCommand(nextOne));
                    this.SortNumber = sortNumber;
                    Global.Execute(new UpdateSysDicItemCommand(this));
                    SysDicViewModel sysDicVm;
                    if (SysDicViewModels.Current.TryGetSysDicVm(this.DicId, out sysDicVm)) {
                        sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItems));
                        sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItemsSelect));
                    }
                }
            });
        }

        public Guid Id {
            get => _id;
            private set {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public Guid DicId {
            get => _dicId;
            set {
                _dicId = value;
                OnPropertyChanged(nameof(DicId));
            }
        }

        public string Code {
            get => _code;
            set {
                if (_code != value) {
                    _code = value;
                    OnPropertyChanged(nameof(Code));
                    if (this == PleaseSelect) {
                        return;
                    }
                    if (string.IsNullOrEmpty(value)) {
                        throw new ValidationException("编码是必须的");
                    }
                    if (SysDicItemViewModels.Current.List.Any(a => a.DicId == this.DicId && a.Code == value && a.Id != this.Id)) {
                        throw new ValidationException("编码重复");
                    }
                }
            }
        }

        public string Value {
            get => _value;
            set {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public string Description {
            get => _description;
            set {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public int SortNumber {
            get => _sortNumber;
            set {
                _sortNumber = value;
                OnPropertyChanged(nameof(SortNumber));
            }
        }

        public SysDicViewModel SysDicVm {
            get {
                SysDicViewModel sysDicVm;
                SysDicViewModels.Current.TryGetSysDicVm(this.DicId, out sysDicVm);
                return sysDicVm;
            }
        }
    }
}

﻿using Planner.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Planner.UI
{
    /// <summary>
    /// Interaction logic for CustomersListControl.xaml
    /// </summary>
    public partial class CustomersListControl : UserControl
    {
        private ICustomersListViewModel _customersListVM;

        public CustomersListControl(ICustomersListViewModel customersListVM)
        {
            InitializeComponent();
            _customersListVM = customersListVM;
            DataContext = customersListVM;
        }
    }
}
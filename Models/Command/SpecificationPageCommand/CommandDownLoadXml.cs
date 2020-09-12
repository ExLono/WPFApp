﻿using LoadData;
using LogicalWork.ManagerSpecification;
using ModelsData.XML;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.ViewModels;

namespace WPFApp.Models.Command.SpecificationPageCommand
{
    public sealed class CommandDownLoadXml : ICommand
    {
        private const string Tag = nameof(CommandDownLoadXml);
        private bool _loadFile;

        private ILoader _loaderXml;
        private IWork _specificationWork;

        public bool CanExecute(object parameter)
        {
            return !_loadFile;
        }

        public void Execute(object parameter)
        {
            var specificationPageViewModel = parameter as SpecificationPageViewModel;
            if (string.IsNullOrEmpty(specificationPageViewModel.Path))
            {
                Log.Log.SetLog(Tag, "Параметр команды оказался пустым!");
                return;
            }
            _loaderXml = new LoaderXml(specificationPageViewModel.Path, new Specification());
            _loaderXml.LoadData();
            _specificationWork = new SpecificationXmlWork(_loaderXml.Model);
            ArrayList arrayListresult;
            Task task = new Task(() =>
            {
                arrayListresult = _specificationWork.GetResultAsync().ArrayList;
                specificationPageViewModel.ResultSpecificationsItems = arrayListresult.Cast<ResultSpecification>();
                _loadFile = false;
            });
            _loadFile = true;
            task.Start();

        }

        //TODO: АНИМАЦИЯ пеоевёртывания клавиши!

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
/*
 * MIT License
 * 
 * Copyright (c) 2024 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Controls;
using Plexdata.WebRequester.GUI.Dialogs;
using Plexdata.WebRequester.GUI.Execution;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Helpers;
using Plexdata.WebRequester.GUI.Helpers.Formatters;
using Plexdata.WebRequester.GUI.Helpers.Importers;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Settings;
using System.Globalization;
using Unity;

namespace Plexdata.WebRequester.GUI;

internal class Program
{
    private IUnityContainer container;

    [STAThread]
    static void Main()
    {
        new Program().Start();
    }

    private void Start()
    {
        this.InitializeProgram();

        Application.Run(this.container.Resolve<MainForm>());
    }

    #region Event handler

    private void OnCurrentDomainUnhandledException(Object sender, UnhandledExceptionEventArgs args)
    {
        sender.ShowError("An unhandled domain exception occurred.", args.ExceptionObject as Exception);
        Application.Exit();
    }

    private void OnApplicationThreadException(Object sender, ThreadExceptionEventArgs args)
    {
        try
        {
            this.container.Resolve<ILogger>().Disaster("An unhandled application thread exception occurred.", args.Exception);
        }
        catch (Exception exception)
        {
            sender.ShowError("An application thread exception occurred that could not be handled.", exception);
            Application.Exit();
        }
    }

    #endregion

    #region Program initialization

    private void InitializeProgram()
    {
        Application.ThreadException += this.OnApplicationThreadException;
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += this.OnCurrentDomainUnhandledException;

        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        this.CreateDependencies();
    }

    private void CreateDependencies()
    {
        if (this.container != null)
        {
            throw new InvalidOperationException();
        }

        this.container = new UnityContainer();

        this.container.RegisterInstance(this.container);

        this.container.RegisterType<ISerializer, JsonSerializer>();
        this.container.RegisterType<IFactory, FactoryHandler>();
        this.container.RegisterSingleton<ISettings<ApplicationSettings>, SettingsManager>();

        this.container.RegisterSingleton<ILogger, LoggingInspector>();

        this.container.RegisterSingleton<IProjectExplorer, ProjectExplorer>();
        this.container.RegisterSingleton<IAboutDialog, AboutDialog>();
        this.container.RegisterType<IRequestDocument, RequestDocument>();
        this.container.RegisterType<ISectionDocument, SectionDocument>();

        this.container.RegisterType<IRequestExecutor, RequestExecutor>();
        this.container.RegisterType<IVariablesReplacer, VariablesReplacer>();
        this.container.RegisterType<IPostmanImporter, PostmanImporter>();
        this.container.RegisterType<IPayloadFormatter, PayloadFormatter>();
        this.container.RegisterType<IJsonFormatter, JsonFormatter>();
        this.container.RegisterType<IXmlFormatter, XmlFormatter>();
        this.container.RegisterType<IHtmlFormatter, HtmlFormatter>();

        this.container.RegisterSingleton<MainForm>();
    }

    #endregion
}
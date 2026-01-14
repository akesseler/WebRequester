/*
 * MIT License
 * 
 * Copyright (c) 2026 plexdata.de
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

using Plexdata.WebRequester.GUI.Dialogs;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Logging;
using Unity;

namespace Plexdata.WebRequester.GUI.Helpers
{
    internal class FactoryHandler : IFactory
    {
        private readonly IUnityContainer container = null;

        public FactoryHandler(IUnityContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public TInstance Create<TInstance>(params Object[] parameters) where TInstance : class
        {
            if (typeof(TInstance) == typeof(ISerializer))
            {
                return this.container.Resolve<ISerializer>() as TInstance;
            }

            if (typeof(TInstance) == typeof(ILoggerEntity))
            {
                return Activator.CreateInstance(typeof(LoggerEntity), parameters) as TInstance;
            }

            if (typeof(TInstance) == typeof(ILoggerEntityDetails))
            {
                return Activator.CreateInstance(typeof(LoggerEntityDetailsDialog), parameters) as TInstance;
            }

            if (typeof(TInstance) == typeof(IModifyLabeledEntity))
            {
                return Activator.CreateInstance(typeof(ModifyLabeledEntityDialog), parameters) as TInstance;
            }

            if (typeof(TInstance) == typeof(IAboutDialog))
            {
                return this.container.Resolve<IAboutDialog>() as TInstance;
            }

            if (typeof(TInstance) == typeof(IProjectExplorer))
            {
                return this.container.Resolve<IProjectExplorer>() as TInstance;
            }

            if (typeof(TInstance) == typeof(IRequestDocument))
            {
                return this.container.Resolve<IRequestDocument>() as TInstance;
            }

            if (typeof(TInstance) == typeof(ISectionDocument))
            {
                return this.container.Resolve<ISectionDocument>() as TInstance;
            }

            if (typeof(TInstance) == typeof(IPostmanImporter))
            {
                return this.container.Resolve<IPostmanImporter>() as TInstance;
            }

            throw new NotSupportedException($"Type of '{typeof(TInstance)}' is not supported by this factory.");
        }
    }
}

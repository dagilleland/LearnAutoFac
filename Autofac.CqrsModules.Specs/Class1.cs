using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
// http://www.ramblingincode.com/building-a-command-pipeline-with-autofac/
// http://devkimchi.com/631/dynamic-module-loading-with-autofac/
// https://github.com/jonstelly/AutofacEvents
// http://docs.autofac.org/en/latest/advanced/adapters-decorators.html

namespace Autofac.CqrsModules.Specs
{
    /* Requirements
     * - One handler per specific command
     * - Generic type as the command handler interface: IHandleCommand<T>
     *      - Passed in to the constructor of the CommandModule that registers the command handlers
     * - Resolved class may handle many different commands
     */
    public class Class1
    {
        [Fact]
        public void Should()
        {
            // Arrange
            var builder = new ContainerBuilder();

            // Act
            builder.RegisterModule(null);

            // Assert
        }
    }
}

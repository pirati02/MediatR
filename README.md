# MediatR

            var services = new ServiceCollection();
            var provider = services.AddMediatR(Assembly.GetExecutingAssembly()).BuildServiceProvider();

            var mediatR = provider.GetRequiredService<IMediatR>();
            var result = await mediatR.SendAsync(new WriteToConsoleRequest
            {
                Text = "Hello Bakar"
            });

path to [Program.cs](https://github.com/pirati02/MediatR/blob/master/Sample/Program.cs) file

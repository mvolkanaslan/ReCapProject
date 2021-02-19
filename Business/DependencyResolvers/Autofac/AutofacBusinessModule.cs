﻿using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        //Katmanların instance larını autofac yapıyor
        protected override void Load(ContainerBuilder builder)
        {
            //Startup daki singleton a karşılık geliyor.
            //Burada Referansları verilen sınıfların menager sınıflarının instance ları gönderiliyor.
            //SingleInstance tek bir instance oluşturarak bütün isteklere cevap verir.
            //SingleInstance data tutmayaz. Operasyonları çalıştırır.
            //Aksi takdirde tüm kullanıcıların dataları aynı instance da tutulursa birbirine karışacaktır.
            builder.RegisterType<CarManager>().As<ICarService>().SingleInstance();
            builder.RegisterType<EfCarDal>().As<ICarDal>().SingleInstance();
            builder.RegisterType<UserManager>().As<IUserService>().SingleInstance();
            builder.RegisterType<EfUserDal>().As<IUserDal>().SingleInstance();
            builder.RegisterType<RentalManager>().As<IRentalService>().SingleInstance();
            builder.RegisterType<EfRentalDal>().As<IRentalDal>().SingleInstance();
            builder.RegisterType<CustomerManager>().As<ICustomerService>().SingleInstance();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>().SingleInstance();
            builder.RegisterType<BrandManager>().As<IBrandService>().SingleInstance();
            builder.RegisterType<EfBrandDal>().As<IBrandDal>().SingleInstance();
            builder.RegisterType<ColorManager>().As<IColorService>().SingleInstance();
            builder.RegisterType<EfColorDal>().As<IColorDal>().SingleInstance();
            //WebAPI nin bu modulle çalışacağını bildirmemiz gerek. Api program.cs de configüre edilecek.

        }
    }
}
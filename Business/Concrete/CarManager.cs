﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        // Kullanılabilecek herhangi bir database servisini burada kullanacağımızı belirliyoruz
        //Ama hangi service olduğunu beilmiyoruz. InMemory de olur MongoDB de olur vb..
        //CarManager sınıfı bu işleri yaptırmak için bir servis kullanmak isteyecek.
        //Bu soyut br servis olmalı ki daha sonra eklenebilecek diğer servisleri entegre atmak onun üzerinden olsun.
        ICarDal _carDal;
        ICarImageService _carImageService;

        public CarManager(ICarDal carDal, ICarImageService carImageService)
        {
            _carDal = carDal;
            _carImageService = carImageService;
        }

        [SecuredOperation("car.add")] // authorization business içine yazılır.
        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Add(Car car)
        {
            _carDal.Add(car);
            return new SuccessResult(Messages.Add_Msg);
        }
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.DeleteMsg);

        }
        [CacheAspect]
        [PerformanceAspect(5)]
        public IDataResult<List<Car>> GetAll()
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(),Messages.ListMsg);
        }

        [CacheAspect]
        public IDataResult<Car> GetById(int id)
        {
            return new SuccessDataResult<Car>(_carDal.GetById(c => c.Id == id),Messages.ListMsg); 
        }

        [CacheAspect]
        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(),Messages.ListMsg);
        }

        public IDataResult<CarDetailDto> GetCarDetaisById(int carId)
        {
            List<CarImage> imageFiles = _carImageService.GetByCarId(carId).Data;
            var carDetails = _carDal.GetCarDetails().Find(car => car.CarId == carId);
            carDetails.CarImages = imageFiles;
            return new SuccessDataResult<CarDetailDto>(carDetails, Messages.ListMsg);
            /*
             ImageService kullanarak CarDetailDTO nesnesine ait olan CarImages listesini set ettim
             */

        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByBrandId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.BrandId == id),Messages.ListMsg);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByColorId(int id)
        {
            
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.ColorId == id), Messages.ListMsg);
        }

        [TransactionScopeAspect]
        public IResult TransactionalOperation(Car car)
        {
            Add(car);
            if(car.Description==null)
            {
                throw new Exception("");
            }
            Add(car);
            return null;
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.UpdateMsg);
        }
    }
}

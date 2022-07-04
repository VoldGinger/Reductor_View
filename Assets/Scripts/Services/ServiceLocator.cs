using System;
using System.Collections.Generic;
using UnityEngine;
namespace Services
{
    public class ServiceLocator : MonoSingleton<ServiceLocator>
    {
        private Dictionary<Type, IService> _services;


        protected override void Awake()
        {
            base.Awake();
            _services = new Dictionary<Type, IService>();
            RegisterServices();
            DontDestroyOnLoad(this);
        }
        private void RegisterServices()
        {
            AddService<SceneLoader>(new SceneLoader());
        }


        public TService GetService<TService>()
        {
            return (TService)_services[typeof(TService)];
        }

        private void AddService<TService>(TService instance) where TService : IService
        {
            _services.Add(instance.GetType(), instance);
        }


    }
}

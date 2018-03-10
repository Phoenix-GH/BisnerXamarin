using System;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.MvvmcrossApp
{
    /// <summary>
    /// Custom container to create views from our storyboard
    /// </summary>
    public class StoryboardContainer : MvxIosViewsContainer
    {
        protected override IMvxIosView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            try
            {
                UIViewController view;

                if (request.ViewModelType == typeof(DashboardViewModel))
                {
                    view = UIStoryboard.FromName("DashboardView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(RoomIndexViewModel))
                {
                    view = UIStoryboard.FromName("RoomIndexView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(DatePickerViewModel))
                {
                    view = UIStoryboard.FromName("DatePickerView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(JobboardViewModel))
                {
                    view = UIStoryboard.FromName("JobboardView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(RoomDetailViewModel))
                {
                    view = UIStoryboard.FromName("RoomDetailView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(RoomTimeIndexViewModel))
                {
                    view = UIStoryboard.FromName("RoomTimeIndexView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(BookingConfirmedViewModel))
                {
                    view = UIStoryboard.FromName("BookingConfirmedView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(TimePickerViewModel))
                {
                    view = UIStoryboard.FromName("TimePickerView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(TimeSelectViewModel))
                {
                    view = UIStoryboard.FromName("TimeSelectView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(WebBrowserViewModel))
                {
                    view = UIStoryboard.FromName("WebBrowserView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(AccessControlViewModel))
                {
                    view = UIStoryboard.FromName("AccessControlView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(LauncherViewModel))
                {
                    view = UIStoryboard.FromName("LauncherView", null).InstantiateViewController(viewType.Name);
                }
                else if (request.ViewModelType == typeof(ResetPasswordViewModel))
                {
                    view = UIStoryboard.FromName("ResetPasswordView", null).InstantiateViewController(viewType.Name);
                }
                else
                {
                    view = UIStoryboard.FromName("MainStoryBoard", null).InstantiateViewController(viewType.Name);
                }

                var mvxTouchView = (IMvxIosView)view;

                return mvxTouchView;
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
                throw;
            }
        }
    }
}
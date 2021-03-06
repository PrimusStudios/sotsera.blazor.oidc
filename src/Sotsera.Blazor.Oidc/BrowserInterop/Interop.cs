﻿// Copyright (c) Alessandro Ghidini. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Based on https://github.com/IdentityModel/oidc-client-js by Brock Allen & Dominick Baier licensed under the Apache License, Version 2.0

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Sotsera.Blazor.Oidc.Core;
using Sotsera.Blazor.Oidc.Core.Protocol.Common.Model;
using Sotsera.Blazor.Oidc.Core.Protocol.SessionManagement.Model;

namespace Sotsera.Blazor.Oidc.BrowserInterop
{
    public class Interop
    {
        private UserManager UserManager { get; set; }
        private IJSRuntime Runtime { get; }
        
        public Interop(IJSRuntime runtime)
        {
            Runtime = runtime;
        }

        internal async Task Init(UserManager userManager)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            await Runtime.InvokeVoidAsync(Consts.Interop.Init, DotNetObjectReference.Create(this));
        }

        public async Task<string> InitSessionFrame(FrameSettings settings)
        {
            return await Runtime.InvokeAsync<string>(Consts.Interop.InitSessionFrame, settings);
        }

        public async Task<string> PostToSessionFrame(string message)
        {
            return await Runtime.InvokeAsync<string>(Consts.Interop.PostToSessionFrame, message);
        }

        public async Task OpenPopup(OidcRequest request)
        {
            await Runtime.InvokeVoidAsync(Consts.Interop.OpenPopup, request);
        }

        public async Task<string> GetAsync<T>(string storageType, string key)
        {
            return await Runtime.InvokeAsync<string>($"{storageType}.getItem", key);
        }

        public async Task SetAsync(string storageType, string key, string value)
        {
            await Runtime.InvokeVoidAsync($"{storageType}.setItem", key, value);
        }

        public async Task RemoveAsync(string storageType, string key)
        {
            await Runtime.InvokeVoidAsync($"{storageType}.removeItem", key);
        }

        [JSInvokable]
        public Task CompleteAuthenticationAsync(string url) => UserManager.CompleteAuthenticationAsync(url);

        [JSInvokable]
        public Task CompleteLogoutAsync(string url) => UserManager.CompleteLogoutAsync(url);
    }
}
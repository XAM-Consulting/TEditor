/*
 * Derivative work of ColorPickerDialog.java from Android SDK API samples,
 * ported to Mono for Android and added .NET style event handling.
 * 
 * Copyright (C) 2007 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;

namespace MonoDroid.ColorPickers
{
    public class RoundColorPickerDialog : Dialog
    {
        public event ColorChangedEventHandler ColorChanged;
        private static Color _initialColor;

        public RoundColorPickerDialog(Context context, Color initialColor)
            : base(context)
        {
            _initialColor = initialColor;
        }

        protected RoundColorPickerDialog(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            _initialColor = Color.Black;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var cPickerView = new RoundColorPickerView(Context, _initialColor);
            cPickerView.ColorChanged += delegate(object sender, ColorChangedEventArgs args)
                                        {
                                            if (ColorChanged != null)
                                                ColorChanged(this, args);

                                            Dismiss();
                                        };
            SetContentView(cPickerView);
            SetTitle("Pick a Color");
        }
    }
}
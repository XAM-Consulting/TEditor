## TEditor for Xamarin

TEditor is a HTML editor for Xamarin, it has so many build-in features and easy to use.

 [![NuGet](https://img.shields.io/nuget/v/TEditor.svg?label=NuGet)](https://www.nuget.org/packages/TEditor) 
 
### Demo
![iOSDemo](https://github.com/XAM-Consulting/TEditor/blob/master/Images/iOS.gif) ![DroidDemo](https://github.com/XAM-Consulting/TEditor/blob/master/Images/Droid.gif)

### Usage
Available on Nuget:[https://www.nuget.org/packages/TEditor/](https://www.nuget.org/packages/TEditor/)

Call single line from any project or PCL, make sure it must be in UIThread.

            TEditorResponse response = await CrossTEditor.Current.ShowTEditor("<p>XAM consulting</p>");
            if (!string.IsNullOrEmpty(response.HTML))
                _displayWebView.Source = new HtmlWebViewSource() { Html = response.HTML };
    		
If user click save it will return html as string. If user click cancel, it return empty string.

### Custom
TEditor allow user to custom toolbar, you can add features when you need it, like

         var toolbar = new ToolbarBuilder().AddBasic().AddH1();
         TEditorResponse response = await CrossTEditor.Current.ShowTEditor("<p>XAM consulting</p>", toolbar);

Also, you can add a new ToolbarItem with new icon like

        var toolbar = new ToolbarBuilder().AddBasic().AddH1("H1Icon.png");

### Features

|Features|      |Platforms|
|:------:|------|---------|
|Bold||Xamarin.iOS|
|Italic||Xamarin.Android|
|Underline|
|Remove format|Basic|
|Justify center|
|Justify full|
|Justify left|
|Justify right|
|H1 ... H6|
|Text color|
|Add unordered list|
|Add ordered list|Standard|
|Subscript|
|Superscript|
|Strikethrough|
|Horizontal rule|
|Indent|
|Outdent|
|Undo|
|Redo|
|Paragraph|All|

### Powered By:
[ZSSRichTextEditor](https://github.com/nnhubbard/ZSSRichTextEditor)

[PopColorPicker](https://github.com/has606/PopColorPicker)

[MonoDroid.ColorPickers](https://github.com/Cheesebaron/MonoDroid.ColorPickers)

# UWP Chips Control

[![NuGet](https://img.shields.io/nuget/v/Yazdipour.UWP.Chips.svg?style=plastic)](https://www.nuget.org/packages/Yazdipour.UWP.Chips)

![ChipControl](https://raw.githubusercontent.com/yazdipour/UWPChipsX/master/docs/images/screenshot.png)
![ChipControl-dark](./docs/images/screenshot-dark.png)

## How to use in a UWP app
If you want to include this control in an app, you can look at the sample project for guidance, but essentially the control has the following bindable properties:

### Xaml:

`xmlns:chipsControl="using:Yazdipour.UWP.Chips"`

## Single Chip
    <chipsControl:Chip Content="Tag2"
                       CloseButtonVisiblity="Collapsed"
                       ChipDelete="Chip_ChipDelete"
                       HorizontalAlignment="Left"/>

## Chips Collection

    <chipsControl:Chips AvailableChips="{x:Bind AvailableOptions}" 
                        SelectedChips="{x:Bind SelectedOptions, Mode=TwoWay}"
                        SelectorStyle="{x:Bind SelectorStyle, Mode=OneWay}"
                        InputVisiblity="Visible"/>

### SelectorStyle (`ChipsControl.ChipsSelectorStyle`)

This control include a selector for adding additional chips. There are 2 styles of selector - `AutoSuggest` and `Selector`

```csharp
ChipsSelectorStyle.AutoSuggest
```

The `AutoSuggest` selector style is based on the UWP AutoSuggest control, so as you type into the search box you will see a list of matches to the items in the `AvailableChips` property (see below). In addition you can simply type a new value. With the AutoSuggest selector style you can add new values that are not yet available.

```csharp
ChipsSelectorStyle.Selector
```

The `Selector` selector style is based on the UWP ComboBox control, so you have to select one of the existing items in the `AvailableChips` property (see below). 

### AvailableChips (`IEnumerable<string>`) 

This property is an enumerable of string values (`IEnumerable<string>`) that represents the options available when selecting a chip. the `Selector` selector style is limited to this list for selecting chips. The `AutoSuggest` selector style can select values from this list or type in a new value not already in `AvailableChips`

### SelectedChips (`IEnumerable<string>`) 

This property is an enumerable of string values (`IEnumerable<string>`) that represents the selected values (chips) from the control


![Image of ChipControl in app](./docs/images/screenshot.gif)

### Forked from: https://github.com/deanchalk/UWPChipsControl
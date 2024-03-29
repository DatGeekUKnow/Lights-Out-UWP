﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Markup;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightsOutUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LightsOutGame game;
        private Color gridColor = Colors.White;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            game = new LightsOutGame();
            CreateGrid();
            DrawGrid();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            string json = JsonConvert.SerializeObject(game);
            ApplicationData.Current.LocalSettings.Values["game"] = json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: LOAD COLOR
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("gridColor"))
            {
                string json = ApplicationData.Current.LocalSettings.Values["gridColor"] as string;

                gridColor = (Color)XamlBindingHelper.ConvertValue(typeof(Color), json);
            }
            else
            {
                gridColor = Colors.White;
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("game"))
            {
                string json = ApplicationData.Current.LocalSettings.Values["game"] as string;
                game = JsonConvert.DeserializeObject<LightsOutGame>(json);
                CreateGrid();
                DrawGrid();
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("gridSize"))
            {
                string json = ApplicationData.Current.LocalSettings.Values["gridSize"] as string;
                if (int.Parse(json) != game.GridSize)
                {
                    game.GridSize = int.Parse(json);
                    CreateGrid();
                    DrawGrid();
                    game.NewGame();
                }
            }
        }

        private void CreateGrid()
        {
            if (boardCanvas == null) return;

            boardCanvas.Children.Clear();
            int rectSize = (int)boardCanvas.Width / game.GridSize;
            // Create rectangles for grid
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = new SolidColorBrush(Colors.White);
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    // Store each row and col as a Point
                    rect.Tag = new Point(r, c);
                    //Register event handler
                    rect.Tapped += Rect_Tapped;
                    rect.RightTapped += Rect_RightTapped;
                    // Put the rectangle at the proper location within the canvas
                    Canvas.SetTop(rect, r * rectSize);
                    Canvas.SetLeft(rect, c * rectSize);
                    // Add the new rectangle to the canvas' children
                    boardCanvas.Children.Add(rect);
                }
            }
        }

        private void Rect_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            game.Cheat();
            DrawGrid();
        }

        private async void Rect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Get row and column from Rectangle's Tag
            Rectangle rect = sender as Rectangle;
            var move = (Point)rect.Tag;
            game.FlipLight((int)move.X, (int)move.Y);
            // TODO: Redraw the board

            // TODO: Show MessageBox if the game is over
            if (game.IsGameOver())
            {
                MessageDialog dialog = new MessageDialog("You've Won!", "Lights Out!");
                dialog.Commands.Add(new UICommand("OK"));

                await dialog.ShowAsync();
                game = new LightsOutGame {GridSize = game.GridSize };
            }
            DrawGrid();

            // Event was handled
            e.Handled = true;
        }

        private void DrawGrid()
        {
            int index = 0;
            // Set the colors of the rectangles
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;
                    if (game.IsOn(r, c))
                    {
                        // On
                        rect.Fill = new SolidColorBrush(gridColor);
                        rect.Stroke = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        // Off
                        rect.Fill = new SolidColorBrush(Colors.Black);
                        rect.Stroke = new SolidColorBrush(gridColor);
                    }
                }
            }
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            game.NewGame();
            DrawGrid();
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            //AboutPage about = new AboutPage();
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1), game.GridSize.ToString());
        }
    }
}
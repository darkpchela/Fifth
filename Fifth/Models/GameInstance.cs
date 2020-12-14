﻿using System;
using System.Collections.Generic;

namespace Fifth.Models
{
    public class GameInstance
    {
        private int moveValue = -1;

        private int movesCount = 0;

        private int[] map = new int[9];

        private List<string> players = new List<string>();

        public string Id { get; }

        public string CurrentPlayer { get; private set; }

        public int PlayersCount
        {
            get
            {
                return players.Count;
            }
        }

        public GameInstance(string id)
        {
            this.Id = id;
        }

        public bool TryMakeMove(int postion, string player)
        {
            if (player != CurrentPlayer || postion < 0 || postion > 8 || map[postion] != 0 || movesCount >= 9)
                return false;
            map[postion] = moveValue;
            SwapMoveValue();
            SwapCurrentPlayer();
            return true;
        }

        public string CalcResult()
        {
            return "Not implemted";
        }

        public void StartGame()
        {
            Random rnd = new Random();
            var index = rnd.Next(0, 1);
            CurrentPlayer = players[index];
        }

        public bool RegistPlayer(string connectionId)
        {
            if (players.Count >= 2)
                return false;
            players.Add(connectionId);
            return true;
        }

        public void KickPlayer(string connectionId)
        {
            players.Remove(connectionId);
        }

        private void SwapMoveValue()
        {
            if (moveValue == -1)
                moveValue = 1;
            else
                moveValue = 1;

            movesCount++;
        }

        private void SwapCurrentPlayer()
        {
            if (CurrentPlayer == players[0])
                CurrentPlayer = players[1];
            else
                CurrentPlayer = players[0];
        }
    }
}
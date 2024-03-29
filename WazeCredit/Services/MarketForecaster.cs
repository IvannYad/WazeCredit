﻿using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class MarketForecaster : IMarketForecaster
    {
        public MarketResult GetMarketResult()
        {
            // Call API to do some complex calculations and current stock market forecast.
            // For now we will hardcode the result.
            return new MarketResult() { MarketCondition = MarketCondition.StableUp };
        }
    }
}

# GBMRepo
 This Is A Library To comunicate with GBM Broker through C# 
 
 Instance of the class:
 ```
 var Trader = new GBM("userEmail", "Password", "Client ID", "Contract ID");
 ```
 Cancel any existing orders:
```
 // Cancel any existing orders
 var AllOrders = Trader.GetAllOrders();
 var PendingOrders = AllOrders.Where(x => x.orderStatus == GBM.OrderState.Pending);
 foreach (var order in PendingOrders)
 {
      Trading.CancelOrder(Convert.ToInt64(order.orderId));
 }
```
Get Acount Financial Data:
```
 var Account = Trader.GetContractBuyingPower();
 Decimal buyingPower = Account.buyingPower;
 Decimal portfolioValue = Account.marketValueTotal;
```
Get Ticker Position in Portfolio:
```
  try
 {
      var currentPosition = Trader.GetPosition(symbol);
      positionQuantity = currentPosition.quantity;
      positionValue = currentPosition.marketValue;
      YieldValue = currentPosition.yieldValue/ currentPosition.marketValue;
                               
 }
 catch (Exception)
 {
    // currentPosition migth return null if no position exist
 }
```
Gets The last 20 prices of ticker (intraday):
```
 var HistoricalDataTicker = Trader.GetTickerHistorical(symbol, 20);
 double avg = HistoricalDataTicker.WeigthedMedian;
 double currentPrice = HistoricalDataTicker.CurrentPrice;
 var LastIndex = HistoricalDataTicker.IntradayPricesList.Last();
 var FisrtIndex = HistoricalDataTicker.IntradayPricesList.FirstOrDefault();
```
Submits Order for Ticker symbol:
```
var symbol ="TSLA";
var positionQuantity = 10;
Decimal currentPrice = 2000;
bool RecurrenceFlag = true;
var orderResponse = Trader.GenerateOrder(symbol, positionQuantity, GBM.GBM.OrderTypes.Sell, currentPrice, GBM.InstrumentTypes.SIC, ref RecurrenceFlag);
  
var orderResponse = Trader.GenerateOrder(symbol, qtyToBuy, GBM.GBM.OrderTypes.Buy, currentPrice, GBM.GBM.InstrumentTypes.SIC, ref RecurrenceFlag);

```                      
ToDo: Port To Python

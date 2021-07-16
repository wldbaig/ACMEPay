# ACMEPay
### Payment processing

Create a RESTful Payment processing API to support the following actions:

- Authorization,
- Void,
- Capture,
- Get all transactions.

#### Authorization request
##### Request:

POST /api/authorize/ 

| Parameter name|Type|Description|
|---|---|---|
|Amount|Decimal|Payment amount|
|Currency|ISO3 String|Payment currency|
|CardHolderName|String|Cardholder name|
|CardNumber|String|PAN – credit card number|
|ExpiryMonth|int|Card expiration month|
|ExpiryYear|int|Card expiration year|
|CVV|int|Card verification value|
|Order Reference|string|Order reference max length 50|

##### Response: 


| Parameter name|Type|Description|
|---|---|---|
|Id|UUID|Payment ID. Exposing database keys should be avoided.|
|Status|Enumeration (Authorized, Captured, Voided)|Authorized|
 
#### Void request

##### Request:

POST /api/authorize/{id}/voids 

| Parameter name|Type|Description|
|---|---|---|
|Id|UUID|Payment ID. received in authorization response. Route parameter|
|Order reference|String|Order reference max length 50, POST body parameter.|
 

##### Response: 
 
| Parameter name|Type|Description|
|---|---|---|
|Id|UUID|Payment ID. Exposing database keys should be avoided.|
|Status|Enumeration (Authorized, Captured, Voided)|Voided|

#### Capture request
##### Request:
POST /api/authorize/{id}/capture 

|Parameter name|Type|Description|
|---|---|---|
|Id|UUID|Payment ID. received in authorization response. Route parameter|
|Order reference|String|Order reference max length 50, POST body parameter.|

##### Response: 
 
| Parameter name|Type|Description|
|---|---|---|
|Id|UUID|Payment ID. Exposing database keys should be avoided.|
|Status|Enumeration (Authorized, Captured, Voided)|Captured|

#### Get all transactions
##### Request:
GET /api/authorize/

Implement pagination.

##### Response: 

| Parameter name|Type|Description|
|---|---|---|
|Amount|Decimal|Payment amount|
|Currency|ISO3 String|Payment currency|
|CardHolderName|String|Cardholder name|
|CardNumber|String|PAN – credit card number, Anonymization should be used. Show only first 6 and last four digits. |
|Order reference|String|Order reference max length 50, POST body parameter.|
|Id|UUID|Payment ID. Exposing database keys should be avoided.| 
|Status|Enumeration (Authorized, Captured, Voided)| Status Enumeration (Authorized, Captured, Voided) Captured.|

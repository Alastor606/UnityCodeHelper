# Extentions methods for comfort Unity programming
## Here I have collected a large number of convenient and useful methods for faster use within Unity. Below will be described, in my opinion, the most interesting and requiring explanations.
### New function - ***Creatnig code assets***
There is two windows :

- #### The script creation window where you need to enter the code and file name for the asset
  
![image](https://github.com/Alastor606/Unity-Code-Helper/assets/114815838/0a0d0979-4c91-415f-8891-04ecb32dcf0d)

- #### Window for creating script assets, here you can specify the file and class name, and also create several instances at once

![image](https://github.com/Alastor606/Unity-Code-Helper/assets/114815838/5f3ec148-28fa-45b9-ab48-2785257b242e)


## The select method, accepts the Func<T> and returns the first item of collection suitable for the condition
```cs
void MyFoo(int[] array)
{
    array.Select((x) => x % 2 == 0).Print(); //Method Print use the Debug.Log for display value in console
}
```
### Next methods is Replace and Swap 
```cs
void MyFoo(int[] array)
{
    array.Replace(2,100);//First `2` value in collection will be replaced to `1000`
    array.Swap(100, 50); //Value 100 will be replaced by 50 will be replaced by previous position 100
}
```
### And the mandatory method to use is AllDo

```cs
void MyFoo(GameObject[] array)
{
    array.AllDo((item) => item.Destroy(2)); // Destroy all GameObjects from array in 2 seconds
}
```
## These were thie most interesting array methods
### Now the methods are directly related to Unity, first is WaitAndDo, works with local coroutine, can be calld from every object Inherited from UnityEngine.Object
```cs
void MyFoo(GameObject someObj)
{
    bool flag = false;
    this.WaitAndDo(2, (currentObj) => flag = true ); //after 2 seconds flag will become true
    someObj.WaitAndDo(5, (item) => item.transform.position = Vector3.Zero);
}
```
### Method ToArray, creates an array where the first element is the given object
```cs
void MyFoo(GameObject someObj)
{
    var objectArray = someObj.ToArray(new GameObject(), new GameObject()); // Take params for the new objects
}
```

### And one of most convenient is the TransferControl2D Method
```cs
void Update()
{
    _rigidobdy.TransferControl2D(_speed, _jumpForce, KeyCode.Space); // Must be called only in Update
}
```

### Also you can move objects along given trajectories bu Bezier curve and just in straight lines
```cs
[SerializeField] private List<Transform> _traectory, _polygonTest;
[SerializeField] private Transform _bezierObject, _movableObject;

void Update()
{
    if (_time < 1) _time += 0.01f;
    else _time = 0;
    _bezierObject.MoveByCurve(_traectory, _time);
    _movableObject.MoveByPolygon(_polygonTest, _time, true);
}
```

### Multi-type array
Use this for create usefull multi-type collections and comfort work with them
```cs
public void MyFoo()
{
    var array = new MTPArray() {123, "string value", 125f};
    array.GetAll<int>().First().value.Print(); //Console output 123
}
```

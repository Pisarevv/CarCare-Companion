import { useEffect, useState } from 'react'
import './App.css'

function App() {
  const [count, setCount] = useState("")

  useEffect(() => {
    (async () => {
    var result = await fetch("https://localhost:7152/Home");
    var result2 = await result.json();
    setCount(count => result2.message);
    })()
  },[])

  return (
    <>
     <h1>{count}</h1>
    </>
  )
}

export default App

import { AuthProvider } from "./contexts/AuthContext";
import AppRouter from "./routes/index";

const App = () => {
  return (
    <>
      <AuthProvider>
        <AppRouter />
      </AuthProvider>
    </>
  )
}

export default App

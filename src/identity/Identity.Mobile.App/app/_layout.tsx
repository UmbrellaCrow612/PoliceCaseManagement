import { Stack } from "expo-router/stack";

export default function Layout() {
  return (
    <Stack>
      <Stack.Screen name="index" options={{ title: "Home" }} />
      <Stack.Screen name="scan" options={{ title: "Scan QR Code" }} />
      <Stack.Screen name="otp" options={{ title: "One time password" }} />
    </Stack>
  );
}

import { useState } from "react";
import { Button, StyleSheet, Text, View } from "react-native";
import {
  CameraView,
  useCameraPermissions,
  BarcodeScanningResult,
} from "expo-camera";
import { useNavigation } from "expo-router";
import AsyncStorage from "@react-native-async-storage/async-storage";

interface OtpData {
  id: string;
  createdAt: string;
  expiresAt: string;
  code: string;
}

export interface TotpData {
  appName: string;
  userName: string;
  secret: string;
  issuer: string;
}

const isValidURI = (string: string) => {
  try {
    new URL(string);
    return true;
  } catch (_) {
    return false;
  }
};

const isOtpData = (data: any): data is OtpData =>
  typeof data === "object" &&
  data !== null &&
  typeof data.id === "string" &&
  typeof data.createdAt === "string" &&
  typeof data.expiresAt === "string" &&
  typeof data.code === "string";

const isTotpData = (url: string): TotpData | undefined => {
  if (!isValidURI(url)) return undefined;

  const { protocol, host, searchParams, pathname } = new URL(url);
  const secret = searchParams.get("secret");
  const issuer = searchParams.get("issuer");
  const [appName, userName] = pathname.substring(1).split(":");

  if (
    protocol !== "otpauth:" ||
    host !== "totp" ||
    !appName ||
    !userName ||
    !secret ||
    !issuer
  ) {
    console.log("Invalid TOTP format");
    return undefined;
  }

  return { appName, userName, secret, issuer };
};

const saveTotpData = async (totpData: TotpData) => {
  try {
    const key = `totp-${totpData.issuer}-${totpData.userName}`;
    await AsyncStorage.setItem(key, JSON.stringify(totpData));

    const existingKeys = await AsyncStorage.getItem("totp-keys");
    const keysArray = existingKeys ? JSON.parse(existingKeys) : [];

    if (!keysArray.includes(key)) {
      keysArray.push(key);
      await AsyncStorage.setItem("totp-keys", JSON.stringify(keysArray));
    }
  } catch (error) {
    console.error("Failed to save TOTP:", error);
  }
};

const processQRCodeData = async (qrString: string) => {
  try {
    const parsedData = JSON.parse(qrString);
    if (isOtpData(parsedData)) return parsedData;
  } catch {}

  const totpData = isTotpData(qrString);
  if (totpData) await saveTotpData(totpData);
  return null;
};

export default function ScanTab() {
  const [permission, requestPermission] = useCameraPermissions();
  const { navigate } = useNavigation<any>();
  const [qrData, setQrData] = useState("");

  const handleScannedData = async (data: string) => {
    const parsedData = await processQRCodeData(data);
    if (parsedData && isOtpData(parsedData)) {
      navigate("otp", parsedData);
    } else if (isTotpData(data)) {
      navigate("index");
    }
  };

  if (!permission || !permission.granted) {
    return (
      <View style={styles.permissionContainer}>
        <Text style={styles.permissionText}>Camera permission required</Text>
        <Button title="Grant Permission" onPress={requestPermission} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <CameraView
        style={styles.camera}
        facing="back"
        barcodeScannerSettings={{ barcodeTypes: ["qr"] }}
        onBarcodeScanned={({ data }: BarcodeScanningResult) =>
          handleScannedData(data)
        }
      >
        <View style={styles.overlay}>
          <View style={styles.roundedBox}>
            <Text style={styles.title}>Scan QR Code</Text>
            {qrData && <Text style={styles.qrDataText}>Data: {qrData}</Text>}
          </View>
        </View>
      </CameraView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: "center", alignItems: "center" },
  camera: { flex: 1, width: "100%" },
  overlay: {
    position: "absolute",
    justifyContent: "center",
    alignItems: "center",
    inset: 0,
  },
  permissionContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(0, 0, 0, 0.6)",
  },
  permissionText: { color: "white", fontSize: 18, marginBottom: 20 },
  roundedBox: {
    width: 280,
    height: 280,
    borderRadius: 20,
    borderWidth: 3,
    borderColor: "#fff",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(0, 0, 0, 0.4)",
  },
  title: { color: "#fff", fontSize: 22, fontWeight: "600", marginBottom: 10 },
  qrDataText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "400",
    textAlign: "center",
    marginTop: 5,
  },
});

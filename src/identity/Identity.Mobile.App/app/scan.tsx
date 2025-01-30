import { useState } from "react";
import { Button, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import {
  CameraView,
  CameraType,
  useCameraPermissions,
  BarcodeScanningResult,
} from "expo-camera";
import { useNavigation } from "expo-router";
import AsyncStorage from "@react-native-async-storage/async-storage";

/*
{
  "id": "123456",
  "createdAt": "2025-01-29T12:00:00Z",
  "expiresAt": "2025-01-30T12:00:00Z",
  "code": "ABC123"
}
use https://qr.io/ for testing

*/

interface OtpData {
  id: string;
  createdAt: Date;
  expiresAt: Date;
  code: string;
}

interface TotpData {
  appName: string;
  userName: string;
  secret: string;
  issuer: string;
}

function isValidURI(string: string) {
  try {
    let l = new URL(string);
    return true;
  } catch (_) {
    return false;
  }
}

function isOtpData(data: any): data is OtpData {
  return (
    typeof data === "object" &&
    data !== null &&
    typeof data.id === "string" &&
    typeof data.createdAt === "string" &&
    typeof data.expiresAt === "string" &&
    typeof data.code === "string"
  );
}

function isTotpData(url: string): TotpData | undefined {
  if (!isValidURI(url)) {
    console.log("Invalid URI for totp");
    return undefined;
  }

  let { protocol, host, searchParams, pathname } = new URL(url);

  let s = searchParams.get("secret");
  let i = searchParams.get("issuer");

  let info = pathname.substring(1).split(":");
  let appName = info[0];
  let userName = info[1];

  if (
    protocol !== "otpauth:" ||
    host !== "totp" ||
    typeof appName !== "string" ||
    appName.length < 1 ||
    typeof userName !== "string" ||
    userName.length < 1 ||
    typeof s !== "string" ||
    typeof i !== "string"
  ) {
    console.log(
      `TOTP bits missing ${protocol} ${host} ${appName} ${userName} ${s} ${i}`
    );
    return undefined;
  }

  let totpData = {
    appName: appName,
    issuer: i,
    secret: s,
    userName: userName,
  } as TotpData;

  return totpData;
}

export default function ScanTab() {
  const [back, _] = useState<CameraType>("back");
  const [permission, requestPermission] = useCameraPermissions();
  const { navigate } = useNavigation<any>();
  const [qrData, setQrData] = useState("");

  const handleTotpCreation = async (totpData: TotpData) => {
    try {
      const key = `totp-${totpData.issuer}-${totpData.userName}`;
      const dataString = JSON.stringify(totpData);

      // Save TOTP entry
      await AsyncStorage.setItem(key, dataString);

      // Update key list
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

  function isValidJSON(string: string) {
    try {
      JSON.parse(string);
      return true;
    } catch (error) {
      return false;
    }
  }

  async function processQRCodeData(
    qrString: string
  ): Promise<OtpData | TotpData | null> {
    try {
      if (isValidJSON(qrString)) {
        if (isOtpData(JSON.parse(qrString))) {
          return JSON.parse(qrString) as OtpData;
        }
        // any other formats that are JSOn we support
      }

      const totpData = isTotpData(qrString);
      if (totpData) {
        await handleTotpCreation(totpData);
      }

      return null; // not a format we support
    } catch (error) {
      console.error("QR Code parsing error:", error);
      return null;
    }
  }

  async function CheckDataFormatAndPush(data: string) {
    const res = await processQRCodeData(data);

    if (!res) {
      return;
    }

    if (isOtpData(res)) {
      navigate("otp", {
        id: res.id,
        createdAt: res.createdAt,
        expiresAt: res.expiresAt,
        code: res.code,
      });
    }
  }

  function onBarcodeScanned(scanningResult: BarcodeScanningResult) {
    CheckDataFormatAndPush(scanningResult.data);
  }

  if (!permission || !permission.granted) {
    return (
      <View style={styles.permissionContainer}>
        <Text style={styles.permissionText}>
          Camera doesn't have permission, please grant it
        </Text>
        <Button title="Request permission" onPress={requestPermission} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <CameraView
        style={styles.camera}
        facing={back}
        barcodeScannerSettings={{
          barcodeTypes: ["qr"],
        }}
        onBarcodeScanned={onBarcodeScanned}
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
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  camera: {
    flex: 1,
    width: "100%",
  },
  overlay: {
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    justifyContent: "center",
    alignItems: "center",
  },
  permissionContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(0, 0, 0, 0.6)",
  },
  permissionText: {
    color: "white",
    fontSize: 18,
    marginBottom: 20,
  },
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
  title: {
    color: "#fff",
    fontSize: 22,
    fontWeight: "600",
    marginBottom: 10,
  },
  qrDataText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "400",
    textAlign: "center",
    marginTop: 5,
  },
});
